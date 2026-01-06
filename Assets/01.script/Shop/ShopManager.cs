using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Shop Settings")]
    [SerializeField] private int cardRemovalCost = 50; // 카드 제거 기본 비용

    /// <summary>
    /// 카드 제거 서비스를 구매합니다.
    /// </summary>
    /// <param name="cardToRemove">제거할 카드 데이터</param>
    public void BuyCardRemoval(CardData cardToRemove)
    {
        // 골드 체크 및 소비
        if (HeroSystem.Instance.SpendGold(cardRemovalCost))
        {
            // 플레이어 데이터(HeroData)의 덱에서 카드 제거
            // HeroSystem을 통해 HeroData에 접근
            // HeroSystem.Instance.RemoveCardFromDeck(cardToRemove);

            Debug.Log($" 카드가 제거되었습니다.");

            // 카드 제거후 한번더 제거하려고 할때 가격이 오름
            cardRemovalCost += 25;
        }
    }

    /// <summary>
    /// 일반 아이템이나 카드를 구매합니다.
    /// </summary>
    public void BuyItem(int price, CardData cardProduct)
    {
        if (HeroSystem.Instance.SpendGold(price))
        {
            // 플레이어 덱에 새 카드 추가
            // HeroSystem.Instance.AddCardToDeck(cardProduct);
            Debug.Log($"구매 완료");

            //구매한 물건은 상점에서 제거 (중복 구매 방지)
            // 해당 버튼을 비활성화 처리
        }
    }
}
