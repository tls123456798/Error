using System.Collections;
using UnityEngine;

/// <summary>
/// 보스전의 버튼 입력과 플레이어 상태를 관리하는 매니저입니다.
/// </summary>
public class BossBattleManager : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private CombatantView playerView;
    [SerializeField] private UnityEngine.UI.Button[] actionButtons;

    [Header("전투 설정")]
    [SerializeField] private int normalAttackDamage = 10;
    [SerializeField] private int superAttackDamage = 25;
    [SerializeField] private int defenseValue = 10;

    private bool isStunned = false; // 강공격 후 스턴 상태

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
        if (!CanAct()) return;

        Debug.Log("플레이어: 강력한 일격! (다음 턴 행동 불가)");

        // 강공격 수행 후 콜백에서 즉시 스턴을 걸고 적에게 턴을 넘깁니다.
        ActionSystem.Instance.Perform(new SuperAttackGA(superAttackDamage), () =>
        {
            isStunned = true;
            if (playerView != null) playerView.SetStunVisual(true);
            EndPlayerTurn();
        });
    }

    /// <summary>
    /// 버튼을 누를 수 있는 상태인지 단순히 '체크'만 합니다.
    /// </summary>
    private bool CanAct()
    {
        // 시스템이 동작 중이거나 기절 상태면 버튼 입력을 무시합니다.
        if (ActionSystem.Instance.IsPerforming) return false;
        if (isStunned) return false;

        return true;
    }

    /// <summary>
    /// 핵심: 적의 공격이 끝난 후 'ActionSystem'이 나에게 턴이 왔음을 알릴 때 호출되어야 합니다.
    /// </summary>
    public void StartPlayerTurn()
    {
        if (isStunned)
        {
            // 로그를 통해 현재 상태를 확실히 파악합니다.
            Debug.Log($"[턴 체크] 현재 기절 상태인가? : {isStunned}");

            if (isStunned)
            {
                isStunned = false; // 먼저 상태를 해제합니다.

                if (playerView != null)
                    playerView.SetStunVisual(false); // 비주얼 복구

                Debug.Log("<color=red>기절 감지!</color> 플레이어 턴을 건너뛰고 즉시 적의 턴을 실행합니다.");

                // 유저의 입력을 기다리지 않고 '즉시' 다시 적의 턴 액션을 수행합니다.
                // ActionSystem이 이전 동작을 완전히 끝냈는지 확인하기 위해 지연 실행을 사용합니다.
                StopAllCoroutines();
                StartCoroutine(AutoSkipRoutine());
            }
            else
            {
                Debug.Log("<color=green>플레이어 행동 가능</color>");
            }
        }
    }

    private IEnumerator AutoSkipRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        EndPlayerTurn();
    }

    private void EndPlayerTurn()
    {
        Debug.Log("시스템: 턴 전환 중...");
        ActionSystem.Instance.Perform(new EnemyTurnGA());
    }

    private void SetButtonsActive(bool active)
    {
        foreach (var btn in actionButtons)
        {
            btn.interactable = active;
        }
    }
}