using UnityEngine;

/// <summary>
/// 플레이어의 덱(HeroData)을 화면에 리스트 형태로 보여주는 UI 매니저입니다.
/// </summary>
public class DeckViewUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject deckPanel; // 덱 전체 패널
    [SerializeField] private Transform contentParent; // 카드가 생성될 Grid(Content) 부모
    [SerializeField] private GameObject cardPrefab; // 화면에 보여줄 카드 프리팹

    [Header("Data Source")]
    [SerializeField] private HeroData heroData; // 내 덱 정보가 들어있는 데이터

    /// <summary>
    /// 덱 버튼을 눌렀을 때 호출하여 창을 엽니다.
    /// </summary>
    public void OpenDeckView()
    {
        if (deckPanel == null) return;

        deckPanel.SetActive(true);
        RefreshDeckDisplay();
    }

    /// <summary>
    /// 현재 HeroData의 덱 리스트를 바탕으로 UI를 새로 그립니다.
    /// </summary>
    private void RefreshDeckDisplay()
    {
        // 사전 체크
        if(contentParent == null || cardPrefab == null)
        {
            Debug.LogError("DeckViewUI: UI 참조가 누락되었습니다! 인스펙터를 확인하세요");
            return;
        }
        
        // 기존에 생성된 카드 UI를 제거 (초기화)
        foreach(Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }
        
        // HeroData의 Deck 리스트를 순회하며 카드 생성
        if(heroData != null && heroData.Deck != null)
        {
            foreach(CardData cardData in heroData.Deck)
            {
                // 프리팹 생성 및 부모 설정
                GameObject cardObj = Instantiate(cardPrefab, contentParent);


                // CardView 컴포넌트 설정
                if(cardObj.TryGetComponent(out CardView cardView))
                {
                    // 우리가 앞서 수정한 CardData를 받는 Setup 함수를 호출합니다.
                    cardView.Setup(cardData);
                }
            }
        }
        else
        {
            Debug.LogWarning("DeckViewUI: 표시할 HeroData나 Deck이 비어있습니다.");
        }
    }

    public void CloseDeckView()
    {
        deckPanel.SetActive(false);
    }
}
