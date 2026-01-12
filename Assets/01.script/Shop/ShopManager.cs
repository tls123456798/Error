using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // 싱글톤 로직
    public static ShopManager Instance { get; private set; }

    [Header("Shop Settings")]
    [SerializeField] private List<CardData> allCardsPool; // 상점에서 팔 수 있는 전체 카드
    [SerializeField] private int cardCount = 3; // 진열할 카드의 개수
    [SerializeField] private int basePrice = 50; // 기본 가격

    [Header("UI References")]
    [SerializeField] private Transform cardParent; // 카드가 배치될 부모(Grid Layout)
    [SerializeField] private GameObject shopCardPrefab; // 아까 만든 UICardView가 포함된 프리팹
    [SerializeField] private GameObject shopGoods; // 상점 판매 요소 그룹 (겹침 방지용 부모)

    [Header("Remove Card Settings")]
    [SerializeField] private int removePrice = 75; // 카드 제거 비용
    [SerializeField] private GameObject deckSelectionPanel; // 내 덱 리스트/제거 패널 부모

    [Header("Remove UI")]
    [SerializeField] private Transform removeListParent; // DeckSelectionPanel 내부의 content

    // 서비스 상태 관리 변수
    private bool isRemoveServiceUsed = false;

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// NPC 클릭 시 호출되어 상점을 초기화하고 상품을 진열합니다.
    /// </summary>
    public void OpenShop()
    {
        // 상점이 열릴 때 기본적으로 판매 물건은 보이고, 제거 패널은 숨김
        if(shopGoods != null) shopGoods.SetActive(true);
        if(deckSelectionPanel != null) deckSelectionPanel.SetActive(false);

        GenerateStock();
    }

    /// <summary>
    /// 판매할 카드를 랜덤하게 생성하여 배치합니다.
    /// </summary>
    private void GenerateStock()
    {
        if(cardParent == null || shopCardPrefab == null) return;

        // 기존 상품 제거
        foreach(Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }

        // 랜덤 카드 선별 (중복 방지를 위해 리스트를 섞음)
        List<CardData> randomCards = allCardsPool
            .OrderBy(g => System.Guid.NewGuid()) // 리스트를 무작위로 석기
            .Take(cardCount) // 그 중 상위 n개 가져오기
            .ToList();

        // UI에 카드 생성
        foreach(CardData data in randomCards)
        {
            GameObject obj = Instantiate(shopCardPrefab, cardParent);

            // UI 초기화 로직
            RectTransform rect = obj.GetComponent<RectTransform>();
            if(rect != null) rect.localScale = Vector3.one;

            // UICardView 스크립트를 통해 데이터와 가격 세팅
            if(obj.TryGetComponent(out UICardView uiCardview))
            {
                // 랜덤한 가격 (예: 40~60골드)
                int finalPrice = basePrice + Random.Range(-10, 11);
                uiCardview.SetupForShop(data, finalPrice);
            }
        }
    }

    /// <summary>
    /// 카드를 클릭했을 때 구매를 시도합니다.
    /// </summary>
    public void TryBuyCard(CardData cardData, int  price, GameObject cardUIObj)
    {
        // HeroSystem을 통해 골드 소모 시도
        // SpendGold는 골드가 부족하면 false, 충분하면 차감 후 true를 반환합니다.
        if (HeroSystem.Instance.SpendGold(price))
        {
            // 구매 성공 시 HeroData의 Deck에 카드 추가
            // HeroSystem에 public HeroData GetHeroData() 함수를 추가하거나 직접 참조)
            HeroSystem.Instance.AddCardToHeroDeck(cardData);

            // 상점 진열대에서 해당 카드 오브젝트 파괴 (품절 처리)
            Destroy(cardUIObj);

            Debug.Log($"구매 성공: 남은 골드: {price} 차감됨.");
        }
        else
        {
            // 구매 실패 (골드 부족)
            Debug.Log("골드가 부족하여 구매할 수 없습니다.");
        }
    }

    /// <summary>
    /// [카드 제거 서비스] 버튼 클릭 시 호출됩니다.
    /// </summary>
    public void OnClickRemoveService()
    {
        // 상점의 물건을 숨기고 제거 하는 패널을 엽니다.
        if(shopGoods != null) shopGoods.SetActive(false);
        if(deckSelectionPanel != null) deckSelectionPanel.SetActive(true);

        RefreshRemoveList();
        Debug.Log("카드 제거 창을 열었습니다.");
    }

    /// <summary>
    /// 제거 창을 닫고 다시 상점 메인으로 돌아갑니다.
    /// </summary>
    public void CloseRemovePanel()
    {
        if(deckSelectionPanel != null) deckSelectionPanel.SetActive(false);
        if(shopGoods != null) shopGoods.SetActive(true);
    }

    // 제거 창이 열릴 때 내 덱의 카드들을 생성하여 보여줌
    public void RefreshRemoveList()
    {
        // 기존에 떠있던 카드 UI들 삭제
        foreach (Transform child in removeListParent) Destroy(child.gameObject);

        // HeroData의 Deck 리스트를 순회하여 카드 생성
        foreach(CardData card in HeroSystem.Instance.GetHeroDeck())
        {
            GameObject obj = Instantiate(shopCardPrefab, removeListParent);

            // 카드 Scale 을 (1, 1, 1)로 강제 고정
            obj.transform.localScale = Vector3.one;

            if(obj.TryGetComponent(out UICardView uiView))
            {
                uiView.Setup(card);
                uiView.currentMode = UICardView.CardMode.DeckRemove; // 제거 모드로 설정
            }
        }
    }

    // 카드를 클릭했을 때 실제 데이터 삭제 요청
    public void RequestRemoveCard(CardData card)
    {
        // 돈이 있는지 확인하고 차감
        if (HeroSystem.Instance.SpendGold(removePrice))
        {
            // 골드 소모 성공 시에만 카드 삭제
            HeroSystem.Instance.RemoveCardFromHeroDeck(card);

            Debug.Log($"{card.name} 제거 완료 및 {removePrice}G 소모됨.");

            // 삭제 후 상점으로 복귀
            CloseRemovePanel();
        }
        else
        {
            Debug.Log("골드가 부족하여 카드를 삭제할 수 없습니다.");
        }
    }
}
