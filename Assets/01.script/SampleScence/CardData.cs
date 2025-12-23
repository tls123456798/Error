using NUnit.Framework;
using SerializeReferenceEditor;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 카드의 기본 정보를 정의하는 ScriptableObject입니다.
/// 프로젝트 창에서 오른쪽 클릭(Data/Card)을 통해 실제 카드 데이터 파일을 생성할 수 있습니다.
/// </summary>
[CreateAssetMenu(menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    [Header("기본 정보")]
    // 카드의 효과난 플레이어 텍스트를 담는 설명한
    [field: SerializeField] public string Description { get; private set; }

    // 카드를 사용하기 위해 소모되는 기본 마나 수치
    [field: SerializeField] public int Mana { get; private set; }

    // 카드 테두리 안에 표시될 캐릭터나 스킬 이미지
    [field: SerializeField] public Sprite Image { get; private set; }

    [Header("효과 설정")]
    /// <summary>
    /// 플레이어가 카드를 드래그하여 직접 타겟을 지어했을 때 발생하는 효과입니다.
    /// (예: 적 한 명에게 10 데미지)
    /// </summary>
    [field: SerializeReference, SR] public Effect MaualTargetEffect { get; private set; } = null;

    /// <summary>
    /// 카드를 낼 때 자동으로 실행되는 추가 효과들의 리스트입니다.
    /// (예: 카드 1장 드로우, 모든 적에게 약화 부여 등 타겟팅이 자동인 효과들)
    /// </summary>
    [field: SerializeField] public List<AutoTargetEffect> OtherEffects {  get; private set; }
}
