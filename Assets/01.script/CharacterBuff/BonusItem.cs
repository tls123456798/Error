using UnityEngine;

// 유니티 메뉴에서 오른쪽 클릭으로 아이템 파일을 만들 수 있게 해 줍니다.
[CreateAssetMenu(fileName = "NewBonus", menuName = "SlayTheSpire/BonusItem")]
public class BonusItem : ScriptableObject
{
    public string itemName; // 아이템 이름
    [TextArea]
    public string description; // 아이템 설명
    public int goldAmount; // 보너스 골드 양
    public int hpBonus; // 보너스 체력 양
}
