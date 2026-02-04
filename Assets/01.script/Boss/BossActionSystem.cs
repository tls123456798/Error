using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActionSystem : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private CombatantView playerView; // 플레이어 컴뱃턴트
    private CombatantView activeBossClone; // 실제 씬에 생성된 보스 클론

    private void Start()
    {
        StartCoroutine(FindActiveBossInstance());
    }

    private IEnumerator FindActiveBossInstance()
    {
        yield return new WaitForSeconds(0.2f);

        EnemyView[] allEnemies = FindObjectsByType<EnemyView>(FindObjectsSortMode.None);
        foreach (var enemy in allEnemies)
        {
            if (enemy.gameObject.activeInHierarchy)
            {
                activeBossClone = enemy;
                Debug.Log($"[BossActionSystem] 활성화된 보스 클론 발견: {activeBossClone.name}");
                break;
            }
        }

        if (activeBossClone == null)
        {
            Debug.LogError("[BossActionSystem] 보스 클론을 찾을 수 없습니다! EnemyBoardView 설정을 확인하세요");
        }
    }

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<AttackGA>(OnAttack);
        ActionSystem.AttachPerformer<SuperAttackGA>(OnSuperAttack);
        ActionSystem.AttachPerformer<DefenseGA>(OnDefense);

        // [추가] 적의 턴이 시작되었을 때 실행할 로직을 등록합니다.
        ActionSystem.AttachPerformer<EnemyTurnGA>(OnEnemyTurn);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AttackGA>();
        ActionSystem.DetachPerformer<SuperAttackGA>();
        ActionSystem.DetachPerformer<DefenseGA>();

        // [추가] 등록 해제
        ActionSystem.DetachPerformer<EnemyTurnGA>();
    }

    // 일반 공격 처리
    private IEnumerator OnAttack(AttackGA action)
    {
        List<CombatantView> targets = new() { activeBossClone };
        DealDamageGA dealDamage = new(action.Damage, targets, playerView);
        ActionSystem.Instance.AddReaction(dealDamage);
        yield return new WaitForSeconds(0.1f);
    }

    // 강공격 처리
    private IEnumerator OnSuperAttack(SuperAttackGA action)
    {
        List<CombatantView> targets = new() { activeBossClone };
        DealDamageGA dealDamage = new(action.Damage, targets, playerView);
        ActionSystem.Instance.AddReaction(dealDamage);

        yield return new WaitForSeconds(0.5f);
    }

    // [핵심 추가] 적(보스)의 턴 행동 처리
    private IEnumerator OnEnemyTurn(EnemyTurnGA action)
    {
        if (activeBossClone == null) yield break;

        Debug.Log("보스의 턴: 플레이어를 공격합니다.");
        yield return new WaitForSeconds(0.5f); // 공격 전 대기 연출

        // 1. 플레이어에게 데미지를 입힙니다.
        List<CombatantView> targets = new() { playerView };
        int bossDamage = 10; // 보스 공격력 설정
        DealDamageGA bossAttack = new(bossDamage, targets, activeBossClone);
        ActionSystem.Instance.AddReaction(bossAttack);

        yield return new WaitForSeconds(0.5f); // 공격 후 대기 연출

        // 2. [가장 중요] 보스의 행동이 끝났으므로 매니저에게 플레이어 턴(혹은 스킵)을 시작하라고 알림
        // 이 함수가 호출되어야 BossBattleManager가 Stun을 확인하고 자동 스킵을 진행합니다.
        BossBattleManager battleManager = FindObjectOfType<BossBattleManager>();
        if (battleManager != null)
        {
            battleManager.StartPlayerTurn();
        }
    }

    // 방어 처리
    private IEnumerator OnDefense(DefenseGA action)
    {
        playerView.AddStatusEffect(StatusEffectType.ARMOR, action.DefenseValue);
        Debug.Log($"플레이어가 {action.DefenseValue}만큼 방어합니다.");
        yield return new WaitForSeconds(0.1f);
    }
}