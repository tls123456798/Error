using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 대상 선택 모드(TargetMode) 중 하나로, 타겟팅이 필요 없는 경우를 정의합니다.
/// 특정 대상을 지정하지 않는 카드 효과(예: 전체 드로우, 전체 마나 회복 등)에 사용됩니다.
/// </summary>
public class NoTM : TargetMode
{
    /// <summary>
    /// 대상 리스트를 가져오는 함수를 재정의(Override)하지만,
    /// 타겟이 없으므로 null을 반환합니다.
    /// </summary>
    /// <returns>타겟이 없을을 의미하는 null</returns>
    public override List<CombatantView> GetTargets()
    {
        // 타겟팅 단계가 생략되거나 필요없는 카드이므로 null을 반환하여 시스템에 알립니다.
        return null;
    }
}
