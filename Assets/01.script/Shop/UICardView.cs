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
    [SerializeField] private Image backGroundImage;

    [Header("Shop Options")]
    [SerializeField] private GameObject priceBadge; // 상점에서만 켤 가격창
    [SerializeField] private TMP_Text priceText;

    public CardData CardData { get; private set; }
    public Card Card { get; private set; }

    // 기본 셋업 (덱 보기 등)
    public void Setup(CardData cardData)
    {
        if (cardData == null) return;

        CardData = cardData;

        if(title != null) title.text = cardData.name;
        if(description != null) description.text = cardData.Description;
        if(mana != null) mana.text = cardData.Mana.ToString();
        if (image != null) image.sprite = cardData.Image;

        if(priceBadge != null) priceBadge.SetActive(false); // 기본은 가격 숨김
    }

    // 게임 실행 중 변경된 상태를 가진 Card 객체를 전달받아 UI를 셋업합니다.
    public void Setup(Card card)
    {
        if(card == null) return;

        Card = card;

        // Card 클래스 내부에 정의도니 프로퍼티 활용
        if (title != null) title.text = card.Title;
        if(description != null) description.text = card.Description;
        if(mana != null) mana.text = card.Mana.ToString();
        if(image != null) image.sprite = card.Image;

        if(priceBadge != null) priceBadge.SetActive(false);
    }

    // 상점용 셋업
    public void SetupForShop(CardData cardData, int price)
    {
        Setup(cardData); // 기본 정보 세팅

        if(priceBadge != null)
        {
            priceBadge.SetActive(true);

            if(priceText != null) priceText.text = price.ToString();
        }
    }
}
