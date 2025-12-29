using UnityEngine;

/// <summary>
/// 액션(GameAction)이 실행되는 시점을 정의하는 열거형입니다.
/// 특정 효과가 이벤트의 전/후 중 언제 발동될지를 결정할 때 사용합니다.
/// </summary>
public enum ReactionTiming
{
    /// <summary>
    /// 액션이 실제로 실행되기 전 단계입니다.
    /// 주로 데미지 감소, 공격 무효화, 예고 연출 등에 사용됩니다.
    /// </summary>
    PRE,

    /// <summary>
    /// 액션이 완전히 실행된 후의 단계입니다.
    /// 주로 반격, 추가 드로우, 사망 확인, 효과 정산 등에 사용됩니다.
    /// </summary>
    POST
}
