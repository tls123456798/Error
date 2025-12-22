using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 대상에게 상태 이상(버프/디버프) 효과를 부여하는 'Effect' 데이터 클래스
/// 인스펙터에서 설정된 데이터를 바탕으로 실제 실행 로직인 'AddStatusEffectGA(GameAction)' 을 생성합니다.
/// </summary>
public class AddStatusEffectEffect : Effect
{
    [Header("상태 이상 설정")]
    [Tooltip("부여할 상태 이상의 종류 (예: 독, 화상, 기절 등)")]
    [SerializeField] private StatusEffectType statusEffectType;

    [Tooltip("한 번에 부여할 중첩(Stack) 횟수")]
    [SerializeField] private int stackCount;

    /// <summary>
    /// 설정된 상태 이상 정보를 담은 실질적인 액션(GameAction) 객체를 생성하여 반환합니다.
    /// </summary>
    /// <param name="targets">상태 이상을 적용받을 대상 리스트</param>
    /// <param name="caster">효과를 시전한 주체</param>
    /// <returns>액션 시스템에서 실행될 상태 이상 부여 액션 객체</returns>
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        // 팩토리 패턴의 일종으로, 설정값(Type, Count)과 런타임 정보(Targets)를 조합
        // 실제 동작 단위인 AddStatusEffectGA를 만들어 냅니다.
        return new AddStatusEffectGA(statusEffectType, stackCount, targets);
    }
}
