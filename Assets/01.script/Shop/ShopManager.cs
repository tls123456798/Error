using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // 싱글톤 로직
    public static ShopManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Shop Settings")]
    [SerializeField] private List<CardData> allCardsPool; // 상점에서 팔 수 있는 전체 카드
    [SerializeField] private int cardCount = 3; // 진열할 카드의 개수
    [SerializeField] private int basePrice = 50; // 기본 가격

    [Header("UI References")]
    [SerializeField] private Transform cardParent; // 카드가 배치될 부모(Grid Layout)
    [SerializeField] private GameObject shopCardPrefab; // 아까 만든 UICardView가 포함된 프리팹

    public void OpenShop()
    {
        GenerateStock();
    }

    private void GenerateStock()
    {
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
}
