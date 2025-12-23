using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 모든 적군을 타겟으로 선택하는 타겟팅 모두(Target Mode) 클래스
/// 광역 공격이나 적 전체 버프/디버프 스킬 등을 구현할 때 사용
/// </summary>
public class AllEnemiesTM : TargetMode
{
    /// <summary>
    /// 현재 전투 현장에 존재하는 모든 적 리스트를 가져옵니다.
    /// </summary>
    /// <returns>EnemySystem에서 관리하는 모든 적군(CombatantView) 리스트</returns>
    public override List<CombatantView> GetTargets()
    {
        // EnemySystem.Instance.enemies 리스트를 바탕으로 새로운 리슽 객체를 생성 반환
        // 원본 리스트를 직접 반환하지 않고 new()로 복사본을 만드는 이유는
        // 외부에서 리스트를 수정하더라도 원본 데이터(EnemySystem)를 보호하기 위함입니다.
        return new(EnemySystem.Instance.Enemies);
    }
}
