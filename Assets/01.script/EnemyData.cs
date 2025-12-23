using UnityEngine;

/// <summary>
/// 적 캐릭터의 기본 능력치 데이터를 관리하는 스크립트 기능입니다.
/// ScriptableObject를 상속받아 파일 형태로 데이터를 저장할 수 있게 해줍니다.
/// </summary>
[CreateAssetMenu(menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    // [fiend: SerializeField]를 사용하면 인스펙터 창에서 편집은 가능하면서,
    // 외부 스크립트에서는 '읽기 전용(private set)' 으로 안전하게 보호할 수 있습니다.
    [field: SerializeField] public Sprite Image { get; private set; } // 적의 외형을 결정하는 이미지(스프라이트)
    [field: SerializeField] public int Health {  get; private set; } // 적의 최대 체력 수치
    [field: SerializeField] public int AttackPower {  get; private set; } // 적이 플레이어에게 입히는 공격력 수치
}
