using UnityEngine;
using UnityEngine.UI;

public class IventorySlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    private ItemData itemData;

    public void Setup(ItemData data)
    {
        itemData = data;
        if(iconImage != null) iconImage.sprite = data.itemIcon;
    }

    // 아이콘(버튼) 클릭 시 실행
    public void OnClickSlot()
    {
        if(itemData == null) return;

        // HeroSystem에 아이템 사용 요청
        HeroSystem.Instance.UseItem(itemData);
    }
}
