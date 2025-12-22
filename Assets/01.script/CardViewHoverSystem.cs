using UnityEngine;


/// <summary>
/// 마우스 호버(Mouse Hover) 시 카드를 확대해서 보여주는 기능을 관리하는 시스템
/// 화면상의 특정 위치에 확대된 카드 뷰를 활성화하거나 비활성화합니다.
/// </summary>
public class CardViewHoverSystem : Singleton<CardViewHoverSystem>
{
    [Header("확대 보기용 카드")]
    [SerializeField] private CardView cardViewHover; // 미리 화면에 배치해둔 확대 전용 CardView 오브젝트

    /// <summary>
    /// 전달받은 카드 데이터를 바탕으로 특정 위치에 확대된 카드를 표시합니다.
    /// </summary>
    /// <param name="card">표시할 카드 데이터 객체</param>
    /// <param name="position">확대된 카드가 나타날 월드 좌표</param>
    public void Show(Card card, Vector3 position)
    {
        // 숨겨져 있던 확대용 카드 오브젝트를 활성화
        cardViewHover.gameObject.SetActive(true);

        // 카드 정보를 텍스트와 이미지로 업데이트 (UI 셋업)
        cardViewHover.Setup(card);

        // 지정된 위치로 이동 (보통 플레이어가 보기 편한 위칠 설정됨)
        cardViewHover.transform.position = position;
    }
    
    /// <summary>
    /// 화면에 표시 중인 확대된 카드를 숨깁니다.
    /// 마우스가 카드 영역을 벗어날 때(OnMouseExit) 주로 호출됩니다.
    /// </summary>
    public void Hide()
    {
       // 확대용 카드 오브젝트를 비활성화하여 화면에서 제거
        cardViewHover.gameObject.SetActive(false);
    }
}
