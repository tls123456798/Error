using UnityEngine;

/// <summary>
/// 휴식처나 이벤트 씬에서 비용 없이 카드를 한 장 선택하여 제거하는 기능을 관리
/// </summary>
public class CardRemovalManager : MonoBehaviour
{
    // 외부에서 접근 가능하도록 싱글톤 설정
    public static CardRemovalManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject removalPanel; // 카드 제거용 패널
    [SerializeField] private Transform contentParent; // 카드가 생성될 부모
    [SerializeField] private GameObject cardPrefab; // 카드 프리팹 (UICardView 포함)

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    // 휴식 씬 등에서 '카드 제거' 버튼을 눌렀을 때 실행
    public void OpenRemovalPanel()
    {
        if(removalPanel != null)
        {
            removalPanel.SetActive(true);
            RefreshList(); // 현재 덱 상태로 리스트 갱신
        }
    }

    /// <summary>
    /// 현재 HeroSystem에 저장된 덱 데이터를 바탕으로 카드 리스트를 생성합니다.
    /// </summary>
    private void RefreshList()
    {
        if(contentParent == null || cardPrefab == null)
        {
            Debug.LogError("CardRemovalManager: UI 참조가 누락되었습니다.");
            return;
        }

        // 기존 UI 초기화
        foreach(Transform child in contentParent) Destroy(child.gameObject);

        // HeroSystem에서 덱을 가져와서 생성
        foreach (CardData card in HeroSystem.Instance.GetHeroDeck())
        {
            GameObject obj = Instantiate(cardPrefab, contentParent);
            obj.transform.localScale = Vector3.one;

            if(obj.TryGetComponent(out UICardView uiView))
            {
                uiView.Setup(card);
                // 카드 제거는 상점 과 달리 돈을 지불하지 않고 제거하는 것임을 알림
                uiView.currentMode = UICardView.CardMode.FreeRemove;
            }
        }
    }

    /// <summary>
    /// 카드를 클릭했을 때 UICardView(FreeRemove 모드)에 의해 호출됩니다.
    /// </summary>
    /// <param name="card">제거할 카드 데이터</param>
    public void OnCardSelected(CardData card)
    {
        if(card == null) return;

        // HeroSystem을 통해 덱에서 카드 제거 (비용 체크 없이 즉시 삭제)
        HeroSystem.Instance.RemoveCardFromHeroDeck(card);

        Debug.Log($"[CardRemoval] 무료 제거 완료: {card.name}");

        // 카드 제거 완료 후 즉시 창을 닫음
        ClosePanel();
    }

    /// <summary>
    /// 제거 창을 닫음
    /// </summary>
    public void ClosePanel()
    {
        if(removalPanel != null)
        {
            removalPanel.SetActive(false);
        }
    }
}
