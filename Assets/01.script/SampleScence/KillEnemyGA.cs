using UnityEngine;

/// <summary>
/// 적 캐릭터의 사망 처리를 담당하는 게임 액션(Game Action) 클래스입니다.
/// 적의 체력이 0이 되었을 때 생성되어 ActionSystem을 통해 실행됩니다.
/// </summary>
public class KillEnemyGA : GameAction
{
    /// <summary>
    /// 사망 처리 대상이 될 적의 View(시각적 객체) 정보입니다.
    /// </summary>
    public EnemyView EnemyView {  get; private set; }

    /// <summary>
    /// KillEnemyGA를 생성할 때 호출되는 생성자입니다.
    /// </summary>
    /// <param name="enemyView">사망할 적 캐릭터의 View</param>
    public KillEnemyGA(EnemyView enemyView)
    {
        // 대상 적 정보를 할당합니다.
        EnemyView = enemyView;
    }
}
