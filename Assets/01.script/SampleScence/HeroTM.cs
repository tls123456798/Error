using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 대상 선택 모드(TargetMode) 중 하나로,
/// 항상 영웅(Hero) 자신만을 타겟으로 반환하는 클래스입니다.
/// 자기 자신에게 버프를 걸거나 힐을 하는 카드/스킬에 주로 사용됩니다.
/// </summary>
public class HeroTM : TargetMode
{
    /// <summary>
    /// 효과가 적용될 리스트를 가져옵니다.
    /// </summary>
    /// <returns>영웅의 View(HeroView)가 담긴 리스트</returns>
    public override List<CombatantView> GetTargets()
    {
        // 타겟들을 담을 새로운 리스트를 생성합니다.
        List<CombatantView> targets = new()
        {
            // HeroSystem 싱글톤 인스턴스를 통해 현재 영웅의 View를 리스트에 추가합니다.
            HeroSystem.Instance.HeroView
        };

        // 영웅 한 명만 담긴 리스트를 반환합니다.
        return targets;
    }
}
