using System;
using UnityEngine;

/// <summary>
/// '적이 영웅을 공격할 때' 라는 조건이 만족되었는지 확인하고 이벤트를 연결하는 클래스입니다.
/// 특정 Perk(특성) 이 발동되는 타이밍을 결정하는 필터 역할을 합니다.
/// </summary>
public class OnEnemyAttackCondition : PerkCondition
{
    /// <summary>
    /// 추가적인 세부 조건이 충족되었는지 확인합니다.
    /// 현재는 단순히 true를 반환하여 적이 공격하기만 하면 무조건 발동되도록 설정되어 있습니다.
    /// </summary>
    /// <param name="gameAction">체크할 게임 액션</param>
    /// <returns>조건 충족 여부</returns>
    public override bool SubConditionIsMet(GameAction gameAction)
    {
        // 예: 특정 공격력이 이상일 때만 발동하게 하려면 여기서 gameAction을 분석하면 됩니다.
        return true;
    }

    /// <summary>
    /// ActionSystem에 '영웅 공격 액션(AttackHeroGA)'이 발생했을 때 실행될 반응을 등록합니다.
    /// </summary>
    /// <param name="reaction">조건이 충족되었을 때 실행할 함수</param>
    public override void SubscribeCondition(Action<GameAction> reaction)
    {
        // AttackHeroGA 액션이 발생할 때(지정된 reactionTiming에) reaction을 호출하도록 구독합니다.
        ActionSystem.SubscribeReaction<AttackHeroGA>(reaction, reactionTiming);
    }

    /// <summary>
    /// 더 이상 조건 감시가 필요 없을 때(예: 특성 해제) 구독을 해제합니다.
    /// </summary>
    /// <param name="reaction">등록했던 반응 함수</param>
    public override void UnsubscribeCondition(Action<GameAction> reaction)
    {
        // 메모리 누수 방지 및 잘못된 호출을 막기 위해 구독을 취소합니다.
        ActionSystem.UnsubscribeReaction<AttackHeroGA>(reaction, reactionTiming);
    }
}
