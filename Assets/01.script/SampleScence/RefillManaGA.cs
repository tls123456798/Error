using UnityEngine;

/// <summary>
/// 플레이어의 마나를 최대치로 다시 채우도록 지시하느 게임 액션 클래스입니다.
/// 특정 데이터를 전달하기보다 '마나 회복' 이라는 이벤트 발생 자체를 알리는 용도로 상용됩니다.
/// </summary>
public class RefillManaGA : GameAction
{
    public int Amount { get; private set; }

    // 생성자: 회복량을 기본값 0으로 설정 (0일 경우 전체 회복으로 활용 가능)
    public RefillManaGA(int amount = 0)
    {
        Amount = amount;
    }
}
