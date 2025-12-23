using DG.Tweening;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 게임의 카드 덱 관리 및 플레이 로직을 담당하는 핵심 시스템
/// 드로우, 패 버리기, 카드 사용 등의 액션을 처리하며 시각적인 연출(DoTween)을 포함합니다.
/// </summary>
public class CardSystem : Singleton<CardSystem>
{
    [Header("시각적 요소")]
    [SerializeField] private HandView handView; // 손패 UI 관리 클래서
    [SerializeField] private Transform drawPilePoint; // 덱(뽑기 더미)의 생성 위치
    [SerializeField] private Transform discardPilePoint; // 버림패 더미으 위치

    // 카드 데이터 관리 리스트
    private readonly List<Card> drawPile = new(); // 뽑을 카드 더미 (덱)
    private readonly List<Card> discardPile = new(); // 이미 사용하거나 버린 카드 더미
    private List<Card> hand = new(); // 현재 플레이어가 손에 들고 있는 카드

    void OnEnable()
    {
        // 액션 시스템에 카드 관련 실행 로직(Performer)들을 등록합니다.
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
    }
    void OnDisable()
    {
        // 시스템 종료 시 메모리 누수 방지를 위해 등록 해제합니다.
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
    }

    /// <summary>
    /// 게임 시작 시 덱 데이터를 기반으로 실제 카드 객체들을 생성합니다.
    /// </summary>
    public void Setup(List<CardData> deckData)
    {
        foreach (var cardData in deckData)
        {
            Card card = new(cardData);
            drawPile.Add(card);

        }
    }

    #region Performers (액션 실행 로직)

    /// <summary>
    /// 카드 드로우 액션을 수행합니다. 덱이 부족하면 버림패를 섞어서 다시 뽑습니다.
    /// </summary>
    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGA)
    {
        int actualAmount = Mathf.Min(drawCardsGA.Amount, drawPile.Count);
        int notDrawAmount = drawCardsGA.Amount - actualAmount;

        // 현재 덱에서 뽑을 수 있는 만큼 뽑기
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }

        // 뽑아야 할 양보다 덱이 부족하면, 버림패를 리필하고 나머지를 마저 뽑기
        if (notDrawAmount > 0)
        {
            RefillDeck();
            for (int i = 0; i < notDrawAmount; i++)
            {
                yield return DrawCard();
            }
        }
    }

    /// <summary>
    /// 손에 있는 모든 카드를 버리는 액션을 수행합니다. (턴 종료 시 등)
    /// </summary>
    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        // 리스트를 순회하며 하나씩 버림 처리
        foreach (var card in new List<Card>(hand)) // 원본 리스트 수정을 피하기 위해 복사본
        {
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        hand.Clear();
    }

    /// <summary>
    /// 플레이어가 카드를 냈을 때의 로직을 처리합니다. 코스트 소모와 효과 발동을 연결합니다.
    /// </summary>
    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        // 손패 데이터 및 UI 제거
        hand.Remove(playCardGA.Card);
        CardView cardView = handView.RemoveCard(playCardGA.Card);
        yield return DiscardCard(cardView);

        // 마나 소모 액션을 반응(Reaction)으로 추가
        SpendManaGA spendManaGA = new(playCardGA.Card.Mana);
        ActionSystem.Instance.AddReaction(spendManaGA);

        // 수동 타겟팅 효과 처리 (플레이어가 직접 고른 대상)
        if (playCardGA.Card.ManualTargetEffect != null)
        {
            PerformEffectGA performEffectGA = new(playCardGA.Card.ManualTargetEffect, new() { playCardGA.ManualTarget });
            ActionSystem.Instance.AddReaction(performEffectGA);
        }

        // 자동 타겟팅 효과들 처리 (예: 모든 적 데미지, 드로우 등)
        foreach (var effectWrapper in playCardGA.Card.OtherEffects)
        {
            List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
            PerformEffectGA performEffectGA = new(effectWrapper.Effect, targets);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }

    #endregion

    #region Internal Logic (내부 동작)


    /// <summary>
    /// 단일 카드 드로우 로직: 데이터를 이동시키로 시각적 카드를 생성하여 패에 넣습니다.
    /// </summary>
    private IEnumerator DrawCard()
    {
        if(drawPile.Count == 0) yield break;

        Card card = drawPile.Draw(); // 리스트의 확장 메서드로 구현 된 것
        hand.Add(card);

        // 카드를 덱 위치에서 생성하여 HandView로 이동시키느 연출 시작
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        yield return handView.AddCard(cardView);
    }

    /// <summary>
    /// 버림패 더미의 카드들을 덱으로 다시 옮깁니다. (보통 여기서 셔플 로직이 포함)
    /// </summary>
    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        // 실무에서는 여기서 drawPile.Shuffle()을 호출해야 합니다.
        discardPile.Clear();
    }

    /// <summary>
    /// 카드가 버려질 때의 시각 연출입니다. 작아지면서 버림패 위치로 이동 후 파괴됩니다.
    /// </summary>
    private IEnumerator DiscardCard(CardView cardview)
    {
        discardPile.Add(cardview.Card);

        cardview.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardview.transform.DOMove(discardPilePoint.position, 0.15f);

        yield return tween.WaitForCompletion();
        Destroy(cardview.gameObject);
    }

    #endregion
}
