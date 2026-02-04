using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 보스전의 버튼 입력과 플레이어 상태를 관리하는 매니저입니다.
/// </summary>
public class BossBattleManager : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private CombatantView playerView;
    [SerializeField] private Button superAttackButton;

    [Header("전투 설정")]
    [SerializeField] private int normalAttackDamage = 10;
    [SerializeField] private int superAttackDamage = 25;
    [SerializeField] private int defenseValue = 10;
    [SerializeField] private int superAttackMaxCooldown = 3;

    private int currentCooldown = 0;

    public void OnAttackButtonClick()
    {
        if (!CanAct()) return;

        Debug.Log("플레이어: 일반 공격 실행");
        ActionSystem.Instance.Perform(new AttackGA(normalAttackDamage), EndPlayerTurn);
    }

    public void OnDefenseButtonClick()
    {
        if (!CanAct()) return;

        Debug.Log("플레이어: 방어 태세 돌입");
        ActionSystem.Instance.Perform(new DefenseGA(defenseValue), EndPlayerTurn);
    }

    public void OnSuperAttackButtonClick()
    {
        // 쿨타임 중이거나 행동 불능이면 리턴
        if (!CanAct() || currentCooldown > 0) return;

        Debug.Log($"플레이어: 강력한 일격! (쿨타임 {superAttackMaxCooldown}턴 발생");

        // 강공격 사용 즉시 쿨타임 설정
        currentCooldown = superAttackMaxCooldown;
        UpdateUI();

        ActionSystem.Instance.Perform(new SuperAttackGA(superAttackDamage), EndPlayerTurn);
    }

    /// <summary>
    /// 버튼을 누를 수 있는 상태인지 단순히 '체크'만 합니다.
    /// </summary>
    private bool CanAct()
    {
        return !ActionSystem.Instance.IsPerforming;
    }

    /// <summary>
    /// 핵심: 적의 공격이 끝난 후 'ActionSystem'이 나에게 턴이 왔음을 알릴 때 호출되어야 합니다.
    /// </summary>
    public void StartPlayerTurn()
    {
        if (currentCooldown > 0)
        {
            currentCooldown--;
            Debug.Log($"강공격 쿨타임 감소 중... 남은 턴: {currentCooldown}");
        }

        UpdateUI();
        Debug.Log("<color=green>플레이어 턴 시작</color>");
    }

    private void UpdateUI()
    {
        if(superAttackButton != null)
        {
            // 쿨타임이 남아 있으면 버튼을 비활성화 합니다.
            superAttackButton.interactable = (currentCooldown <= 0);
        }
    }
    private void EndPlayerTurn()
    {
        Debug.Log("시스템: 턴 전환 중...");
        ActionSystem.Instance.Perform(new EnemyTurnGA());
    }
}