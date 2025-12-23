using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Splines; // 유니티 Spline 패키지 사용

/// <summary>
/// 플레이어의 손패(Hand)에 있는 카드들의 시각적인 배치와 움직임을 관리합니다.
/// Spline을 따라 카드를 곡선 형태로 정렬해주는 기능을 포함합니다.
/// </summary>

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer; // 카드가 배치될 곡선 경로
    private readonly List<CardView> cards = new(); // 현재 손에 들고 있는 카드 리스트

    /// <summary>
    /// 손에 새로운 카드를 추가하고 위치를 다시 정렬합니다.
    /// </summary>
    public IEnumerator AddCard(CardView cardView)
    {
        cards.Add(cardView);
        // 카드가 추가되었으니 바뀐 개수에 맞춰 위치 갱신 (0.15초 동안 이동)
        yield return UpdateCardPositions(0.15f);
    }
    /// <summary>
    /// 특정 카드를 손에서 제거하고 남은 카드들을 정렬합니다.
    /// </summary>
    public CardView RemoveCard(Card card)
    {
        CardView cardView = GetCardView(card);
        if (cardView == null) return null;

        cards.Remove(cardView);
        // 카드가 빠진 빈자리를 채우기 위해 위치 갱신 (비동기 실행)
        StartCoroutine(UpdateCardPositions(0.15f));
        return cardView;
    }
    /// <summary>
    /// 데이터 모델(Card)에 해당하는 시각적 객체(CardView)를 리스트에서 찾아냅니다.
    /// </summary>
    private CardView GetCardView(Card card)
    {
        // LINQ를 사용하여 조건에 맞는 첫 번째 카드를 찾습니다.
        return cards.Where(cardView => cardView.Card == card).FirstOrDefault();
    }

    /// <summary>
    /// Spline 경로를 기반으로 모든 카드의 목표 위치와 회전값을 계산하여 부드럽게 이동시킵니다.
    /// </summary>
    /// <param name="duration">이동에 걸리는 시간</param>
    private IEnumerator UpdateCardPositions(float duration)
    {
        if (cards.Count == 0) yield break;

        // 카드 간격 설정 (Spline의 전체 길이 1을 기준으로 0.1만틈의 간격)
        float cardSpacing = 1f / 10f;

        // 카드들이 중앙에 오도록 첫 번째 카드의 위치(t 값) 계산
        // 0.5(중앙)를 기준으로 카드 개수의 절반만큼 왼쪽에서 시작합니다.
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;

        Spline spline = splineContainer.Spline;

        for (int i = 0; i < cards.Count; i++)
        {
            // 현재 카드가 위치해야 할 Spline상의 비율(0~1 사이의 p값) 계산
            float p = firstCardPosition + i * cardSpacing;

            // Spline에서 해당 위치의 좌표, 접선(방향), 위쪽 방향 데이터를 가져옵니다.
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);

            // 카드가 카메라를 향하거나 곡선의 흐름에 맞게 회전하도록 계산합니다.
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);

            // DOTween을 이용한 부드러운 이동 및 회전
            cards[i].transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back, duration);
            cards[i].transform.DORotate(rotation.eulerAngles, duration);
        }
        // 모든 애니메이션이 진행되는 동안 대기
        yield return new WaitForSeconds(duration);
    }
}
