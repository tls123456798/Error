using DG.Tweening;
using System;
using UnityEngine;

/// <summary>
/// 게임 내에서 카드(CardView) 오브젝트를 생성하는 역할을 전담하는 팩토리 클래스
/// 싱글톤으로 구성되어 어디서든 카드를 생성하고 연출을 부여할 수 있습니다.
/// </summary>
public class CardViewCreator : Singleton<CardViewCreator>
{
    [Header("프리팹 설정")]
    [SerializeField] private CardView cardViewPrefab; // 복사해서 사용할 카드 원본 프리팹


    /// <summary>
    /// 카드 데이터를 바탕으로 실제 월드에 카드 오브젝트를 생성하고 초기 연출을 실행합니다.
    /// </summary>
    /// <param name="card">생성할 카드의 데이터 객체</param>
    /// <param name="position">생성될 초기 위치 (주로 덱 위치)</param>
    /// <param name="rotation">생성될 초기 회전값</param>
    /// <returns>생성된 CardView 컴포넌트</returns>
    public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation)
    {
        // 카드 프리팸을 지정된 위치와 회전값으로 복제(Instantiate)
        CardView cardView = Instantiate(cardViewPrefab, position, rotation);

        // 초기 스케일을 0으로 설정 (등장 연출 준비)
        cardView.transform.localScale = Vector3.zero;

        // DoTween을 사용하여 0.15초 동안 크기가 1로 커지는 연출 실행
        cardView.transform.DOScale(Vector3.one, 0.15f);

        // 생성된 카드 뷰에 실제 데이터(Card)를 주입하여 텍스트/이미지 셋업
        cardView.Setup(card);

        return cardView;

    }

    /// <summary>
    /// (미구현) 데이터 없이 위치 정보만으로 생성할 때 사용하는 메서드
    /// </summary>
    internal CardView CreateCardView(Vector3 position, Quaternion identity)
    {
        // 아직 로직이 구현되지 않았음을 알리느 예외 처리
        throw new NotImplementedException();
    }
}
