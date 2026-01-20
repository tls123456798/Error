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
            ApplyEffect();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
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
