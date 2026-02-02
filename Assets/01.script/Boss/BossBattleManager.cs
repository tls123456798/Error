using UnityEngine;

/// <summary>
/// 보스전의 버튼 입력과 플레이어 상태를 관리하는 매니저입니다.
/// </summary>
public class BossBattleManager : MonoBehaviour
{
    [Header("전투 설정")]
    [SerializeField] private int normalAttackDamage = 10;
    [SerializeField] private int superAttackDamage = 25;
    [SerializeField] private int defenseValue = 10;

    private bool isStunned = false; // 강공격 후 스턴

    public void OnAttackButtonClick()
    {
        if(!CanAct()) return;

        Debug.Log("플레이어: 일반 공격 실행");
        // AttackGA 액션을 생성하여 시스템에 전달
        ActionSystem.Instance.Perform(new AttackGA(normalAttackDamage), EndPlayerTurn);
    }

    public void OnDefenseButtonClick()
    {
        if (!CanAct()) return;

        Debug.Log("플레이어: 방어 태세 돌입");
        // DefenseGA 액션을 생성하여 시스템에 전달
        ActionSystem.Instance.Perform(new DefenseGA(defenseValue), EndPlayerTurn);
    }

    public void OnSuperAttackButtonClick()
    {
        if (!CanAct()) return;

        Debug.Log("플레이어: 강력한 일격! (다음 턴 행동 불가");
        isStunned = true; // 강공격 사용 시 스턴 예약
        ActionSystem.Instance.Perform(new SuperAttackGA(superAttackDamage), EndPlayerTurn);
    }

    // 플레이어가 현재 행동 가능한 상태인지 확인합니다.
    private bool CanAct()
    {
        // 시스템이 이미 무언가를 수행 중이면 중복 클릭 방지
        if(ActionSystem.Instance.IsPerforming) return false;

        // 스턴 상태라면 턴을 강제로 종료 합니다.
        if (isStunned)
        {
            Debug.Log("플레이어는 기절 상태입니다! 턴을 넘깁니다.");
            isStunned = false; // 상태해제
            EndPlayerTurn();
            return false;
        }

        return true;
    }

    // 플레이어의 행동이 끝난 후 호출되어 적의 턴을 시작합니다.
    private void EndPlayerTurn()
    {
        Debug.Log("플레이어 턴 종료 -> 적 턴 시작");
        ActionSystem.Instance.Perform(new EnemyTurnGA());
    }
}
