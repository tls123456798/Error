using UnityEngine;

/// <summary>
/// 게임 내에 존해하는 상태 이상(버프/디버프)의 종류를 정의합니다.
/// 새로운 상태 이상(예: 중독, 기절 등)이 필요한 경우 여기에 항목을 추가합니다.
/// </summary>
public enum StatusEffectType
{
    /// <summary>
    /// 방어도: 적의 공격으로부터 체력을 보호하는 수치입니다.
    /// 보통 턴이 시작될 때 초기화되거나 특정 액션에 의해 감소합니다.
    /// </summary>
    ARMOR,

    /// <summary>
    /// 화상: 매 턴마다 지속적인 데미지를 입히는 상태 이상입니다.
    /// 중첩(Stack) 수치에 따라 데미지가 결정될 수 있습니다.
    /// </summary>
    BURN
}
