using TMPro;
using UnityEngine;

/// <summary>
/// 물리적인 카드 오브젝트의 시각적 표현과 마우스 상호작용(드래그, 호버)을 관리합니다.
/// 플레이어의 입력을 받아 실제 게임 액션(PlayCardGa)을 생성하는 창구 역할을 합니다.
/// </summary>
public class CardView : MonoBehaviour
{
    [Header("UI 구성 요소")]
    [SerializeField] private TMP_Text title; // 카드 이름 텍스트
    [SerializeField] private TMP_Text description; // 카드 설명 텍스트
    [SerializeField] private TMP_Text mana; // 마나 비용 텍스트
    [SerializeField] private SpriteRenderer imageSR; // 카드 일러스트 이미지
    [SerializeField] private GameObject wrapper; // 카드의 외형을 담은 부모 오브젝트 (호버 시 숨기기용)

    [Header("상호작용 설정")]
    [SerializeField] private LayerMask dropLayer; // 카드를 사용 가능한 영역(필드 등)을 판별하는 레이어
    public Card Card { get; private set; } // 이 뷰가 표현하고 있는 실제 카드 데이터 

    private Vector3 dragStartPosition; // 드래그 시작 시 원본 위치
    private Quaternion dragStartRotation; // 드래그 시작 시 원본 회전값

    /// <summary>
    /// 카드 데이터를 받아 시각적인 요소(텍스트, 이미지)를 셋업합니다.
    /// </summary>
    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        imageSR.sprite = card.Image;

    }

    #region Mouse Events (마우스 상호작용)

    /// <summary>
    /// 마우스를 카드 위에 올렸을 때 (호버 연출)
    /// </summary>
    private void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;

        wrapper.SetActive(false); // 현재 카드를 숨김
        Vector3 pos = new(transform.position.x, -2, 0);
        CardViewHoverSystem.Instance.Show(Card, pos); // 별도의 호버용 UI 시스템을 통해 크게 보여줌
    }

    /// <summary>
    /// 마우스가 카드를 벗어났을 때
    /// </summary>
    private void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;

        CardViewHoverSystem.Instance.Hide(); // 호버 UI를 끄고
        wrapper.SetActive(true); // 원래 카드 외형을 다시 보이게 함
    }

    /// <summary>
    /// 카드를 클릭했을 때 (드래그 혹은 타겟팅 시작)
    /// </summary>
    void OnMouseDown()
    {
      
        if (!Interactions.Instance.PlayerCanInteract()) return;

        // 수동 타겟팅(대상 지정)이 필요한 카드인 경우
        if (Card.ManualTargetEffect != null)
        {
           
            ManualTargetSystem.Instance.StartTargeting(transform.position); // 화살표 시스템 시작
        }
        // 대상을 지정하지 않는(필드 드롭형) 카드인 경우
        else
        {
            Interactions.Instance.PlayerIsDragging = true;
            wrapper.SetActive(true);
            CardViewHoverSystem.Instance.Hide();

            // 원래 자리 정보를 저장 (실패 시 복귀용)
            dragStartPosition = transform.position;
            dragStartRotation = transform.rotation;

            // 드래그 중에는 회전을 풀고 마우스 위치로 즉시 이동
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
        }
    }

    /// <summary>
    /// 카드를 드래그 중일 때
    /// </summary>
    void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null) return; // 타겟팅형 카드는 드래그하지 않음

        // 마우스 위치를 따라 카드가 움직임
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }

    /// <summary>
    /// 마우스 버튼을 땟을 때 (카드 사용 시도)
    /// </summary>
    void OnMouseUp()
    {
      
        if (!Interactions.Instance.PlayerCanInteract()) return;

       // 수동 타겟팅 카드 처리
        if (Card.ManualTargetEffect != null)
        {
            
            EnemyView target = ManualTargetSystem.Instance.EndTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));

            // 타겟이 존재하고 마나가 충분하다면 실행
            if (target != null && ManaSystem.Instance.HasEnoughMana(Card.Mana))
            {
                PlayCardGA playCardGA = new(Card, target);
                ActionSystem.Instance.Perform(playCardGA);
            }
            // 드래그 드롭형 카드 처리
            else
            {
                // 마나가 충분하고 드롭 가능한 레이어 위에 있다면 실행
                if (ManaSystem.Instance.HasEnoughMana(Card.Mana)
               && Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropLayer))
                {
                    PlayCardGA playCardGA = new(Card);
                    ActionSystem.Instance.Perform(playCardGA);
                }
                else
                {
                    // 조건 불충족 시 원래 패의 위치로 복구
                    transform.position = dragStartPosition;
                    transform.rotation = dragStartRotation;
                }
                Interactions.Instance.PlayerIsDragging = false;
            }
        }
    }

    #endregion
}
