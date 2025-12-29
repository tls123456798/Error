using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬이나 아이템의 효과가 적용될 대상을 결정하는 타겟팅 시스템의 기초 클래스입니다.
/// [System.Serializable]이 붙어 있어 인스펙터에서 직렬화하여 데이터를 설정할 수 있습니다.
/// </summary>
[System.Serializable]
public abstract class TargetMode
{
    /// <summary>
    /// 현재 타겟팅 규칙에 따라 대상(CombatantView)들의 리스트를 반환합니다.
    /// 자식 클래스에서 구체적인 타겟팅 로직(한 면, 랜덤, 전체 등)을 구현해야 합니다.
    /// </summary>
    /// <returns>선택된 전투원(CombatantView) 리스트</returns>
    public abstract List<CombatantView> GetTargets();
}
