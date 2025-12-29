using UnityEngine;

/// <summary>
/// 플레이어가 카드를 사용하는 행동(Action)을 정의하는 데이터 클래스입니다.
/// 어떤 카드를 썼는지, 그리고 (필요하다면) 어떤 적을 타겟팅했는지에 대한 정보를 담습니다.
/// </summary>
public class PlayCardGA : GameAction
{
    /// <summary>
    /// 유저가 드래그를 통해 직접 지정한 적 타겟입니다.
    /// 타겟이 필요 없는 카드의 경우 null이 될 수 있습니다.
    /// </summary>
    public EnemyView ManualTarget { get; private set; }

    /// <summary>
    /// 플레이어에 의해 사용된 카드 객체입니다.
    /// </summary>
    public Card Card { get; set; }

    /// <summary>
    /// [생성자] 타겟 지정이 필요 없는 카드를 사용할 때 호출됩니다.
    /// </summary>
    /// <param name="card">사용된 카드</param>
    public PlayCardGA(Card card)
    {
        Card = card;
        ManualTarget = null;
    }

    /// <summary>
    /// [생성자] 특정 적을 타겟으로 지정하여 카드를 사용할 때 호출됩니다.
    /// </summary>
    /// <param name="card">사용된 카드</param>
    /// <param name="target">유저가 선택한 대상(적)</param>
    public PlayCardGA(Card card, EnemyView target)
    {
        Card = card;
        ManualTarget = target;
    }
}
