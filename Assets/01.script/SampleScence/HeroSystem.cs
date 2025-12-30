using System;
using UnityEngine;

/// <summary>
/// 게임 내에서 영웅의 상태를 관리하고 전체적인 전투 로직(턴 처리, 반응 동작 등)을 실행하는 핵심 시스템 클래스입니다.
/// 싱글톤으로 구성되어 어디서든 접근 가능하며, HeroData를 받아 HeroView를 초기화합니다.
/// </summary>
public class HeroSystem : Singleton<HeroSystem>
{
    // 영웅의 외형 및 UI를 담당하는 HeroView에 대한 참조입니다.
    [field: SerializeField] public HeroView HeroView { get; private set; }

    [Header("Player Gold")]
    [SerializeField] private int gold = 100; // 초기 골드 설정

    // 재화가 변경되었을 때 UI 등에 알림을 보내기 위한 이벤트
    public event Action<int> OnGoldChanged;

    /// <summary>
    /// 현재 보유한 골드 양 (읽기 전용)
    /// </summary>
    public int CurrentGold => gold;

    /// <summary>
    /// 오브젝트가 활성화될 때 실행됩니다.
    /// ActionSystem에 적의 턴이 시작될 때와 끝날 때의 반응(Reaction)을 등록합니다.
    /// </summary>
    void OnEnable()
    {
        // 적 턴 시작 전(PRE): 내 패를 모두 버리는 동작을 예약합니다
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyturnPreReaction, ReactionTiming.PRE);
        // 적 턴 종료 후(POST): 화상 데미지 처리 및 카드 드로우를 예약합니다.
        ActionSystem.SubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    /// <summary>
    /// 오브젝트가 비활성화될 때 실행됩니다. (메모리 누수 방지를 위해 구독을 해제합니다.)
    /// </summary>
    void OnDisable()
    {
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyturnPreReaction, ReactionTiming.PRE);
        ActionSystem.UnsubscribeReaction<EnemyTurnGA>(EnemyTurnPostReaction, ReactionTiming.POST);
    }

    /// <summary>
    /// 골드를 추가합니다. (전투 승리 보상 등)
    /// </summary>
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"골드 획득: {amount} / 현재 골드: {gold}");
        OnGoldChanged?.Invoke(gold); // 구독 중인 UI가 있다면 업데이트 알림
    }
    public bool SpendGold(int amount)
    {
        if(gold >= amount)
        {
            gold -= amount;
            OnGoldChanged?.Invoke(gold);
            return true;
        }

        Debug.Log("골드가 부족합니다.");
        return false;
    }

    /// <summary>
    /// 외부(예: 배틀 매니저)에서 HeroData를 전달받아 영웅의 초기 설정을 진행합니다.
    /// </summary>
    /// <param name="heroData">설정할 영웅의 데이터 에셋</param>
    public void Setup(HeroData heroData)
    {
        HeroView.Setup(heroData);
    }

    /// <summary>
    /// 적의 턴이 시작되기 직전에 실행되는 로직입니다.
    /// </summary>
    private void EnemyturnPreReaction(EnemyTurnGA enemyTurnGA)
    {
        // 모든 카드를 버리는 액션(DiscardAllCardsGA)를 생성하여 시스템에 추가합니다.
        DiscardAllCardsGA discardAllCardsGA = new();
        ActionSystem.Instance.AddReaction(discardAllCardsGA);
    }

    /// <summary>
    /// 적의 턴이 완전히 종료된 후 실행되는 로직입니다.
    /// </summary>
    private void EnemyTurnPostReaction(EnemyTurnGA enemyTurnGA)
    {
        // 화상(BURN) 상태 이상이 있는지 확인하고 있다면 데미지 액션을 추가합니다.
        int burnStacks = HeroView.GetStatusEffectStacks(StatusEffectType.BURN);
        if(burnStacks > 0)
        {
            ApplyBurnGA applyBurnGA = new(burnStacks, HeroView);
            ActionSystem.Instance.AddReaction(applyBurnGA);
        }
        // 새로운 턴을 위해 카드 5장을 뽑는 액션(DrawCardsGA)을 추가합니다.
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.AddReaction(drawCardsGA);
    }
}
