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

    [Header("사운드 설정")]
    [Tooltip("카드를 사용할 때 재생될 효과음")]
    [SerializeField] private AudioClip useSound;
    public AudioClip UseSound => useSound; // 외부에서 읽기 전용으로 접근

    [Header("효과 설정")]
    [field: SerializeReference, SR] public Effect MaualTargetEffect { get; private set; } = null;

    [field: SerializeField] public List<AutoTargetEffect> OtherEffects {  get; private set; }
}
