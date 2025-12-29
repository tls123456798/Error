using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 대상 선택 모드(TargetMode) 중 하나로, 현재 전투 중인 적들 중
/// 무작위로 한 명을 선택하는 기능을 담당합니다.
/// </summary>
public class RandomEnemyTM : TargetMode
{
    /// <summary>
    /// EnemySystem에 등록된 모든 적들 중 임의의 대상 하나를 리스트에 담아 반환합니다.
    /// </summary>
    /// <returns>무작위로 선택된 적이 포함된 리스트</returns>
    public override List<CombatantView> GetTargets()
    {
        // 1. EnemySystem에서 관리하는 전체 적 리스트(Enemies) 중 무작위 인덱스를 선택합니다.
        // Random.Range(min, max)에서 정수형은 max가 제외되므로 리스트의 Count를 그대로 사용합니다.
        int randomIndex = Random.Range(0, EnemySystem.Instance.Enemies.Count);

        // 2. 선택된 인덱스에 해당하는 적(CombatantView)을 가져옵니다.
        CombatantView target = EnemySystem.Instance.Enemies[randomIndex];

        // 3. TargetMode는 항상 리스트 형태를 반환해야 하므로, 선택된 단일 대상을 새 리스트에 넣어 반환합니다.
        return new List<CombatantView>() { target };
    }
}
