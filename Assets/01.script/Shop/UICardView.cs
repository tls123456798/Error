using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICardView : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private Image image;

    [Header("Shop Options")]
    [SerializeField] private GameObject priceBadge; // 상점에서만 켤 가격창
    [SerializeField] private TMP_Text priceText;

    public CardData Data { get; private set; }

    // 기본 셋업 (덱 보기 등)
    public void Setup(CardData cardData)
    {
        Data = cardData;
        title.text = cardData.ToString();
        description.text = cardData.Description;
        mana.text = cardData.Mana.ToString();
        image.sprite = cardData.Image;

        if(priceBadge != null) priceBadge.SetActive(false); // 기본은 가격 숨김
    }

    // 상점용 셋업
    public void SetupForShop(CardData cardData, int price)
    {
        Setup(cardData);
        if(priceBadge != null)
        {
            priceBadge.SetActive(true);
            priceText.text = priceBadge.ToString();
        }
    }
}
