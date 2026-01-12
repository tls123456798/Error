using UnityEngine;
using UnityEngine.EventSystems;

public class ShopNPC : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel; // 뛰어줄 상점 UI 패널

    // 마우스로 NPC를 클릭했을 때 실행됩니다.
    private void OnMouseDown()
    {
        // 마우스가 UI(버튼, 패널 등) 위에 있다면 아래 로직을 실행하지 않고 리턴합니다.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // 상점을 여는 로직
        if(shopPanel != null)
        {
            shopPanel.SetActive(true);
            // 상점이 열릴 때 상품이 새로 배치하도록 명령합니다.
            ShopManager.Instance.OpenShop();
        }
    }
}
