using UnityEngine;
using SerializeReferenceEditor;

/// <summary>
/// '누구에게(TargetMode)'와 '무엇을(Effect)' 할 것인지를 하나의 ㅆ아으로 묶어주는 데이터 클래스
/// 주로 스킬 시스템이나 아이템 효과 설정에서 타겟팅과 효과를 동시에 정의할 때 사용
/// </summary>
[System.Serializable]
public class AutoTargetEffect
{
    /// <summary>
    /// 효과가 적용될 대상을 판별하는 모드
    /// [SerializeReference]를 통해 TargetMode를 상속받은 다양한 클래서(예: AllEnemiesTM)을
    /// 인스펙터에서 유연하게 할당할 수 있습니다.
    /// </summary>
    [field: SerializeReference, SR] public TargetMode TargetMode {  get; private set; }

    /// <summary>
    /// 대상에게 실행할 구체적인 효과 입니다.
    /// 마찬가지로 Effect를 상속받은 다양한 효과 (예: AddStatusEffectEffect)를 할당할 수 있습니다.
    /// </summary>
    [field: SerializeReference, SR] public Effect Effect { get; private set; }
}
