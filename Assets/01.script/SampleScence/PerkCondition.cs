using System;
using UnityEngine;

/// <summary>
/// 특성(Perk)이 발동되기 위한 '조건' 들의 기초가 되는 추상 클래스입니다.
/// 모든 세부 조건 클래스(예: 공격 시, 사망 시 등)는 이 클래스를 상속받아야 합니다.
/// </summary>
public abstract class PerkCondition
{
    // 효과가 실행될 시점(PRE: 익션 전, POST: 액션 후)을 결정합니다.
    // 자식 클래스에서 접근할 수 있도록 protected로 선언되었습니다.
    [SerializeField] protected ReactionTiming reactionTiming;

    /// <summary>
    /// ActionSystem에 특정 이벤트를 감시하도록 등록하는 함수입니다.
    /// 자식 클래스에서 어떤 액션(GA)을 감시할지 구체적으로 구현해야 합니다.
    /// </summary>
    /// <param name="reaction">조건 충족 시 실행될 콜백 함수</param>
    public abstract void SubscribeCondition(Action<GameAction> reaction);

    /// <summary>
    /// 등록했던 이벤트 감시를 해제하는 함수입니다.
    /// 메모리 누수를 방지하기 위해 반드시 구현되어야 합니다.
    /// </summary>
    /// <param name="reaction">등록했던 콜백 함수</param>
    public abstract void UnsubscribeCondition(Action<GameAction> reaction);

    /// <summary>
    /// 기본적인 이벤트 발생 외에 추가적인 세부 조건(예: 확률, 특정 체력 이하 등)이
    /// 만족되었는지 최종 확인하는 함수입니다.
    /// </summary>
    /// <param name="gameAction">체크할 게임 액션 데이터</param>
    /// <returns>최종 발동 가능 여부</returns>
    public abstract bool SubConditionIsMet(GameAction gameAction);
}
