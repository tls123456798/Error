using UnityEngine;

/// <summary>
/// '화상 데미지 적용' 액션에 필요한 데이터를 관리하는 클래스
/// 매 턴 시작 시 또는 특정 조건에서 화상 상태 이상에 의한 데미지를 처리할 때 생성
/// </summary>
public class ApplyBurnGA : GameAction
{
    // 이번 액션으로 적용될 구체적인 화상 데미지 수치
    public int BurnDamage {  get; private set; }

    // 화상 데미지를 입을 대상 (전투원)
    public CombatantView Target {  get; private set; }

    /// <summary>
    /// 화상 데미지 액션 데이터를 생성
    /// </summary>
    /// <param name="burnDamage">적용할 데미지 양</param>
    /// <param name="target">데미지를 입을 대상</param>
    public ApplyBurnGA(int burnDamage, CombatantView target)
    {
        // 외부에서 데이터를 함부로 변경할 수 없도록 Read-Only(private set) 속성에 값을 할당합니다.
        BurnDamage = burnDamage;
        Target = target;
    }
}
