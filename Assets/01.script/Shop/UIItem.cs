using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;

    private ItemData currentItemData;

    public void Setup(ItemData data)
    {
        currentItemData = data;
        iconImage.sprite = data.itemIcon;
        nameText.text = data.name;
        priceText.text = $"{data.price}G";
    }

    // 아이템 구매 버튼에 연결할 함수
    public void OnBuyButtonClick()
    {
        if (HeroSystem.Instance.SpendGold(currentItemData.price))
        {
            // 아이템 획득 처리
            HeroSystem.Instance.AddItem(currentItemData);

            Debug.Log($"{currentItemData.itemName}을(를) 구매하여 보관함에 넣었습니다.");
            Destroy(gameObject); // 상점 진열대에서 제거
        }
    }

    private void ApplyEffect()
    {
        switch (currentItemData.itemType)
        {
            case ItemType.HP_Potion:
                HeroSystem.Instance.UpdateHealth(currentItemData.recoveryAmount);
                break;
            case ItemType.Mana_Potion:
                RefillManaGA refillManaGA = new RefillManaGA(currentItemData.recoveryAmount);
                ActionSystem.Instance.Perform(refillManaGA);
                break;
        }
    }
}
