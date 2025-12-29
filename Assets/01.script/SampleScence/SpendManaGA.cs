using UnityEngine;

/// <summary>
/// 플레이어의 마나를 특정 수치만큼 소모하도록 지시하는 게임 액션 클래스입니다.
/// 카드 사용 비용이나 능력 활성화 비용 등을 처리할 때 사용됩니다.
/// </summary>
public class SpendManaGA : GameAction
{
    /// <summary>
    /// 소모할 마나의 양입니다.
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// SpendManaGA 액션을 생성하며 소모할 마나 수치를 설정합니다.
    /// </summary>
    /// <param name="amount">소모할 마나량</param>
    public SpendManaGA(int amount)
    {
        Amount = amount;
    }
}
