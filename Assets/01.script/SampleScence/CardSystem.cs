using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임의 카드 덱 관리 및 플레이 로직을 담당하는 핵심 시스템.
/// 드로우, 패 버리기, 카드 사용 등의 액션을 처리하며 시각적인 연출을 포함합니다.
/// </summary>
public class CardSystem : Singleton<CardSystem>
{
    [Header("시각적 요소")]
    [SerializeField] private HandView handView; // 손패 UI 관리 클래스
    [SerializeField] private Transform drawPilePoint; // 덱 생성 위치
    [SerializeField] private Transform discardPilePoint; // 버림패 위치

    // 카드 데이터 관리 리스트
    private readonly List<Card> drawPile = new(); // 덱
    private readonly List<Card> discardPile = new(); // 버림패
    private List<Card> hand = new(); // 손패

    // [추가] 드로우 중복 실행 방지를 위한 플래그
    private bool isProcessingDraw = false;

    void OnEnable()
    {
        // 액션 시스템에 카드 관련 실행 로직들을 등록합니다.
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
    }

    /// <summary>
    /// 새로운 전투 시작 시 덱 데이터를 기반으로 시스템을 초기화합니다.
    /// </summary>
    public void Setup(List<CardData> deckData)
    {
        // 1. 이전 전투의 흔적 지우기
        StopAllCoroutines();
        isProcessingDraw = false;

        drawPile.Clear();
        discardPile.Clear();
        hand.Clear();

        // 2. HandView(시각적 요소) 초기화 호출
        if (handView != null)
        {
            handView.ClearHand();
        }

        // 3. 덱 생성
        foreach (var cardData in deckData)
        {
            Card card = new(cardData);
            drawPile.Add(card);
        }

        Debug.Log("CardSystem: 새로운 전투 셋업 완료");
    }

    #region Performers (액션 실행 로직)

    /// <summary>
    /// 카드 드로우 액션을 수행합니다. 중복 실행 및 덱 부족 상황을 방어합니다.
    /// </summary>
    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGA)
    {
        // [중요] 이미 드로우 중이라면 중복된 명령은 무시합니다.
        if (isProcessingDraw)
        {
            Debug.LogWarning("CardSystem: 드로우가 이미 진행 중입니다. 명령을 건너뜁니다.");
            yield break;
        }

        isProcessingDraw = true;

        // 한 번에 뽑는 양을 최대 5장으로 제한 (안전장치)
        int totalToDraw = Mathf.Min(drawCardsGA.Amount, 5);

        for (int i = 0; i < totalToDraw; i++)
        {
            // 덱이 비었을 때의 처리
            if (drawPile.Count == 0)
            {
                if (discardPile.Count > 0)
                {
                    RefillDeck();
                    yield return new WaitForSeconds(0.2f); // 리필 연출 시간
                }
                else
                {
                    break; // 더 이상 뽑을 카드가 없음
                }
            }

            // 한 장씩 순차적으로 뽑고 연출이 끝날 때까지 기다립니다.
            yield return StartCoroutine(DrawCard());
        }

        isProcessingDraw = false;
    }

    /// <summary>
    /// 손에 있는 모든 카드를 버리는 액션을 수행합니다.
    /// </summary>
    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        // 원본 수정을 피하기 위해 복사본 생성
        List<Card> cardsToDiscard = new List<Card>(hand);

        foreach (var card in cardsToDiscard)
        {
            CardView cardView = handView.RemoveCard(card);
            if (cardView != null)
            {
                yield return StartCoroutine(DiscardCard(cardView));
            }
        }
        hand.Clear();
    }

    /// <summary>
    /// 플레이어가 카드를 냈을 때의 로직을 처리합니다.
    /// </summary>
    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        // playCardGa.Card.Data를 통해 CardData에 접근하여 설정된 소리를 재생합니다.
        if(playCardGA.Card.UseSound != null)
        {
            // 카메라 위치에서 소리 발생
            AudioSource.PlayClipAtPoint(playCardGA.Card.UseSound, Camera.main.transform.position);
        }

        hand.Remove(playCardGA.Card);
        CardView cardView = handView.RemoveCard(playCardGA.Card);

        if (cardView != null)
        {
            yield return StartCoroutine(DiscardCard(cardView));
        }

        // 마나 소모 및 효과 실행 (AddReaction을 통해 순차적 처리)
        ActionSystem.Instance.AddReaction(new SpendManaGA(playCardGA.Card.Mana));

        if (playCardGA.Card.ManualTargetEffect != null)
        {
            ActionSystem.Instance.AddReaction(new PerformEffectGA(playCardGA.Card.ManualTargetEffect, new() { playCardGA.ManualTarget }));
        }

        foreach (var effectWrapper in playCardGA.Card.OtherEffects)
        {
            List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
            ActionSystem.Instance.AddReaction(new PerformEffectGA(effectWrapper.Effect, targets));
        }
    }

    #endregion

    #region Internal Logic (내부 동작)

    private IEnumerator DrawCard()
    {
        if (drawPile.Count == 0) yield break;

        Card card = drawPile.Draw(); // 확장 메서드 사용
        hand.Add(card);

        // 카드 뷰 생성 및 핸드 뷰 배치 대기
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        yield return handView.AddCard(cardView);
    }

    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        // 필요 시 여기서 drawPile.Shuffle(); 추가
        discardPile.Clear();
        Debug.Log("CardSystem: 버림패를 덱으로 리필했습니다.");
    }

    private IEnumerator DiscardCard(CardView cardview)
    {
        if (cardview == null) yield break;

        discardPile.Add(cardview.Card);

        // DoTween 연출
        cardview.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardview.transform.DOMove(discardPilePoint.position, 0.15f);

        yield return tween.WaitForCompletion();

        if (cardview != null)
        {
            Destroy(cardview.gameObject);
        }
    }

    #endregion
}