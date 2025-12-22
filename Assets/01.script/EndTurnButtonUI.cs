using UnityEngine;

/// <summary>
/// '턴 종료' 버튼 UI의 동작을 관리하는 클래스입니다.
/// 버튼 클릭 시 플레이어의 턴을 마치고 적의 턴(EnemyTurnGA)을 실행하도록 시스템에 요청합니다.
/// </summary>
public class EndTurnButtonUI : MonoBehaviour
{
    /// <summary>
    /// UI 버튼의 OnClick 이벤트에 연결되는 메서드입니다.
    /// </summary>
    public void OnClick()
    {
        // 적의 턴을 수행하기 위한 새로운 액션 객체를 생성합니다.
        EnemyTurnGA enemyTurnGA = new();

        // 액션 시스템에 이 액션을 즉시 수행하도록 명령합니다.
        // 이를 통해 현재 플레이어의 행동이 멈추고, 등록된 적의 턴 로직(Performer)이 시작됨니다.
        ActionSystem.Instance.Perform(enemyTurnGA);
    }
}
