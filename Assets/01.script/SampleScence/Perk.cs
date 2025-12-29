using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내에서 실제로 동작하는 특성(Perk) 객체입니다.
/// 특정 조건(Condition)을 감시하다가 조건이 맞으면 효과(Effect)를 실행합니다.
/// </summary>
public class Perk
{
    // 특성의 아이콘 이미지를 데이터로 부터 가져옵니다.
    public Sprite Image => data.Image;

    private readonly PerkData data; // 특성 원본 데이터
    private readonly PerkCondition condition; // 발동 조건 (예: 공격 시, 턴 종료 시 등)
    private readonly AutoTargetEffect effect; // 발동될 효과와 타겟팅 정보

    /// <summary>
    /// PerData를 기반으로 실제 동작할 Perk 객체를 생성합니다.
    /// </summary>
    public Perk(PerkData perkData)
    {
        data = perkData;
        condition = data.PerkCondition;
        effect = data.AutoTargetEffect;
    }

    /// <summary>
    /// 특성이 플레이어에게 추가될 때 호출됩니다.
    /// 조건 감시(이벤트 구독)를 시작합니다.
    /// </summary>
    public void OnAdd()
    {
        condition.SubscribeCondition(Reaction);
    }

    /// <summary>
    /// 특성이 제거될 때 호출됩니다.
    /// 메모리 누수 방지를 위해 조건 감시(이벤트 구독)를 해제합니다.
    /// </summary>
    public void OnRemove()
    {
        condition.UnsubscribeCondition(Reaction);
    }

    /// <summary>
    /// 감시하던 조건이 발생했을 때 실행되는 실제 반응 로직입니다.
    /// </summary>
    /// <param name="gameAction">발생한 게임 액션 정보</param>
    private void Reaction(GameAction gameAction)
    {
        // 세부 조건(예: 확률, 특정 상태 등)이 만족되는지 확인합니다.
        if (condition.SubConditionIsMet(gameAction))
        {
            List<CombatantView> targets = new();

            // 만약 액션을 일으킨 시전자(Caster)를 타겟으로 삼아야 한다면 (예: 반격 등)
            if(data.UseActionCasterAsTarget && gameAction is IHaveCaster haveCaster)
            {
                targets.Add(haveCaster.Caster);
            }

            // 자동 타겟팅 설정이 되어 있다면 해당 타겟들을 리스트에 추가합니다.
            if (data.UseAutoTarget)
            {
                targets.AddRange(effect.TargetMode.GetTargets());
            }

            // 설정된 효과(Effect)와 타겟들을 조합하여 새로운 GameAction을 생성합니다.
            // 시전자는 영웅(HeroView)으로 고정되어 있습니다.
            GameAction perkEffectAction = effect.Effect.GetGameAction(targets, HeroSystem.Instance.HeroView);

            // 생성된 액션을 액션 시스템의 반응(Reaction) 큐에 추가하여 실행 대기시킵니다.
            ActionSystem.Instance.AddReaction(perkEffectAction);
        }
    }
}
