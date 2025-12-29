using DG.Tweening.Core.Easing;
using SerializeReferenceEditor;
using UnityEngine;

/// <summary>
/// 특성(Perk)의 설정 데이터를 저장하는 ScriptableObject입니다.
/// 프로젝트 창에서 우크릭으로 생성하여 다양한 특성 에셋을 만들 수 있습니다.
/// </summary>
[CreateAssetMenu(menuName ="Data/Perk")]
public class PerkData : ScriptableObject
{
    // 특성의 아이콘으로 사용될 이미지입니다.
    [field: SerializeField] public Sprite Image { get; private set; }

    /// <summary>
    /// 특성이 언제 발동될지 결정하는 조건부 데이터입니다.
    /// </summary>
    [field: SerializeReference, SR] public PerkCondition PerkCondition { get; private set; }

    /// <summary>
    /// 조건 만족 시 실행될 효과와 타겟팅 정보입니다.
    /// 마찬가지로 다향성을 활용하여 다양한 효과를 연결할 수 있습니다.
    /// </summary>
    [field: SerializeReference, SR] public AutoTargetEffect AutoTargetEffect { get; private set; }

    /// <summary>
    /// 효과 적용 시 AutoTargetEffect에 설정된 타겟팅 방식을 사요할지 여부입니다.
    /// </summary>
    [field: SerializeField] public bool UseAutoTarget { get; private set; } = true;

    /// <summary>
    /// 효과 적용 시 이 이벤트를 일으킨 시전자(Caster)를 타겟으로 포함할지 여부입니다.
    /// 반격(공격받았을 때 공격자를 탁세으로 함) 기능을 만들 때 유용합니다.
    /// </summary>
    [field: SerializeField] public bool UseActionCasterAsTarget { get; private set; } = false;
}
