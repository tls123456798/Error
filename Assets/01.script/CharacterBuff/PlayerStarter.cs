using UnityEngine;

public class PlayerStarter : MonoBehaviour
{
    private void Start()
    {
        // 금고에서 아까 저장한 아이템 정보를 꺼내옵니다.
        BonusItem picked = GameDateManager.Instance.selectedBonus;

        if(picked != null)
        {
            Debug.Log($"{picked.itemName}을(를) 가지고 게임을 시작합니다!");
            // 여기서 플레이어의 골드를 늘리거나 체력을 높여주는 로직을 실행합니다.
        }
    }
}
