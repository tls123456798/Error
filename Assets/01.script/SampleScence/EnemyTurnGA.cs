using UnityEngine;

/// <summary>
/// "적의 턴이 시작되었다"는 것을 알리는 데이터 클래스입니다.
/// GameAction을 상속받아 ActionSystem을 통해 전달되는 '메시지' 혹은 '명령' 역할을 합니다.
/// </summary>
public class EnemyTurnGA : GameAction
{
    // 현재는 내용이 비어있지만, 이 클래스의 '타입' 자체가
    // EnemySystem에게 "이제 네가 행동할 차례야" 라고 알려주는 스위치 역할을 합니다.

    // 나중에 필요한 경우, 몇 번째 턴인지(int turnCount) 등의 추가 정보를 담을 수 있습니다.
}
