using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어의 마나 수치를 관리하고 관련 UI를 업데이트하는 시스템입니다.
/// 마나 소미(Spend)와 회복(Refill) 액션을 처리하며 싱글톤으로 운영됩니다.
/// </summary>
public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI manaUI; // 마나를 화면에 표시해줄 UI 컴포넌트 참조
    private const int MAX_MANA = 3; // 최대 마나 한도 (상수)
    private int currentMana = MAX_MANA; // 현재 보유 마나

    /// <summary>
    /// 시스템 활성화 시, 마나 관련 액션 실행기(Performer)와 턴 종료 반응(Reaction)을 등롭합니다.
    /// </summary>
    void OnEnable()
    {
        // SpendManaGa 액션이 발생하면 SpendManaPerformer 로직을 실행하도록 연결합니다.
        ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer);
        // RefillManaGA 액션이 발생하면 RefillManaPerformer 로직을 실행하도록 연결합니다.
        ActionSystem.AttachPerformer<RefillManaGA>(RefillManaPerformer);
        // 적의 턴이 끝났을 때(POST) 마나를 회복하기 위해 이벤트를 구독합니다.
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    /// <summary>
    /// 시스템 비활성화 시, 등록했던 실행기와 구독한 이벤트를 해제하여 메모리 누수를 방지합니다.
    /// </summary>
    void OnDisable()
    {
        ActionSystem.DetachPerformer<SpendManaGA>();
        ActionSystem.DetachPerformer<RefillManaGA>();
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    /// <summary>
    /// 현재 보유한 마나가 요청한 양보다 충분한지 확인합니다.
    /// </summary>
    /// <param name="mana">필요한 마나량</param>
    /// <returns>충분하면 true, 부족하면 false</returns>
    public bool HasEnoughMana(int mana)
    {
        return currentMana >= mana;
    }

    /// <summary>
    /// [실행기] 마나를 실제로 감소시키고 UI를 갱신합니다.
    /// </summary>
    private IEnumerator SpendManaPerformer(SpendManaGA spendManaGA)
    {
        currentMana -= spendManaGA.Amount; // 전달받은 양만큼 마나 차감
        manaUI.UpdateManaText(currentMana); // UI 업데이트
        yield return null; // 한 프레임 대기 (연출이 필요할 경우 시간을 늘리 수 있음)
    }

    /// <summary>
    /// 마나를 최대치고 회복 시키고 UI를 갱신합니다.
    /// </summary>
    private IEnumerator RefillManaPerformer(RefillManaGA refillManaGA)
    {
        currentMana = MAX_MANA; // 마나를 최대치로 재설정
        manaUI.UpdateManaText(currentMana); // UI 업데이트
        yield return null;
    }

    /// <summary>
    /// [반응] 적의 턴이 종료되면 플레이어의 마나를 회복하는 액션을 추가합니다.
    /// </summary>
    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        RefillManaGA refillManaGA = new();
        ActionSystem.Instance.AddReaction(refillManaGA); // 마나 회복 액션을 큐에 추가
    }
}
