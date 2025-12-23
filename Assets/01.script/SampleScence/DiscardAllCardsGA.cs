using UnityEngine;

/// <summary>
/// '손에 든 모든 카드 버리기' 액션을 정의하는 클래스입니다.
/// 특별한 매개변수 없이 이 액션의 존배만으로 CardSystem에서 모든 손패를 버리는 로직을 트리거 합니다.
/// 주로 턴 종료 시점에 생성되어 실행됩니다
/// </summary>
public class DiscardAllCardsGA : GameAction
{
    // 현재는 전달할 추가 데이터가 없으므로 비어 있는 상태를 유지합니다.
    // 나중에 '버린 카드 수만큼 데미지' 같은 로직이 필요하다면 
    // CardSystem에서 처리 후 이 클래스에 결과값을 담도록 확장할 수 있습니다.
}
