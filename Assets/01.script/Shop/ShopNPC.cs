using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel; // 뛰어줄 상점 UI 패널

    // 마우스로 NPC를 클릭했을 때 실행됩니다.
    private void OnMouseDown()
    {
        if(shopPanel != null)
        {
            shopPanel.SetActive(true);
            // 상점이 열릴 때 상품이 새로 배치하도록 명령합니다.
            ShopManager.Instance.OpenShop();
        }
    }
}
