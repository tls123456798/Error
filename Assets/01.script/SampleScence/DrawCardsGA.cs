using UnityEngine;


/// <summary>
/// '카드 드로우(Draw)' 액션에 필요한 데이터를 담고 있는 클래스입니다.
/// </summary>
public class DrawCardsGA : GameAction
{
    /// <summary>
    /// 이번 액션으로 뽑을 카드이 총 장수입니다.
    /// </summary>
    public int Amount {  get; set; }

    /// <summary>
    /// 드로우 액션 데이터를 생성합니다.
    /// </summary>
    /// <param name="amount">뽑을 카드의 장수</param>
    public DrawCardsGA(int amount)
    {
        // 외부에서 설정된 장수를 Amount 프로퍼티에 할당합니다.
        Amount = amount;
    }
}
