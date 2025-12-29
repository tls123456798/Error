using UnityEngine;

/// <summary>
/// 유저의 입력 및 상호작용 가능 여부를 관리하는 매니저 클래스입니다.
/// 싱글톤으로 구성되어 게임 어디서든 현재 조작 가능한 상태인지 체크할 수 있습니다.
/// </summary>
public class Interactions : Singleton<Interactions>
{
    /// <summary>
    /// 현재 플레이어가 카드를 드래그 중인지 나타내는 상태 값입니다.
    /// </summary>
    public bool PlayerIsDragging { get; set; } = false;

    /// <summary>
    /// 플레이어가 카드 사용이나 버튼 클릭 등 상호작용을 할 수 있는 상태인지 확인합니다.
    /// </summary>
    /// <returns>상호작용 가능하면 true, 액션 연출 중이라 불가능하면 false</returns>
    public bool PlayerCanInteract()
    {
        // ActionSystem에서 현재 어떤 동작(카드 효과, 연출 등)을 수행 중이 아닐 때만 true를 반환합니다.
        if (!ActionSystem.Instance.IsPerforming) return true;
        else return false;
    }

    /// <summary>
    /// 플레이어가 카드 위에 마우스를 올리는(Hover) 등의 시작적 반응을 할 수 있는지 확인합니다.
    /// </summary>
    /// <returns>카드를 드래그 중이 아니면 true, 드래그 중이면 false</returns>
    public bool PlayerCanHover()
    {
        // 이미 카드를 드래그하고 있다면 다른 카드에 마우스를 올려도 반응하지 않도록 막습니다.
        if(PlayerIsDragging) return false;
        return true;
    }
}
