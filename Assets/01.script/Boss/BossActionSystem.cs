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
        // 씬 시작 시 실제 보스 클론을 찾는 루틴 시작
        StartCoroutine(FindActiveBossInstance());
    }

    /// <summary>
    /// EnemyBoardView에 의해 생성된 활성화된 보스 클론을 찾아 타겟으로 설정합니다.
    /// </summary>
    private IEnumerator FindActiveBossInstance()
    {
        // 보스가 생성되어 활성화될 때까지 잠시 대기
        yield return new WaitForSeconds(0.2f);

        // 씬 내의 모든 EnemyView 중 활성화된 개체(Clone)를 찾습니다.
        EnemyView[] allEnemies = FindObjectsByType<EnemyView>(FindObjectsSortMode.None);
        foreach(var enemy in allEnemies)
        {
            // Hierarchy에서 활성화되어 있고, 원본(비활성화 권장)이 아닌 개체를 선택
            if (enemy.gameObject.activeInHierarchy)
            {
                activeBossClone = enemy;
                Debug.Log($"[BossActionSystem] 활성화된 보스 클론 발견: {activeBossClone.name}");
                break;
            }
        }

        if(activeBossClone == null)
        {
            Debug.LogError("[BossActionSystem] 보스 클론을 찾을 수 없습니다.! EnemyBoardView 설정을 확인하세요");
        }
    }

    private void OnEnable()
    {
        // 버튼 액션들에 대한 실행 로직 (Performer)을 등록합니다.
        ActionSystem.AttachPerformer<AttackGA>(OnAttack);
        ActionSystem.AttachPerformer<SuperAttackGA>(OnSuperAttack);
        ActionSystem.AttachPerformer<DefenseGA>(OnDefense);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AttackGA>();
        ActionSystem.DetachPerformer<SuperAttackGA>();
        ActionSystem.DetachPerformer<DefenseGA>();
    }

    // 일반 공격 처리
    private IEnumerator OnAttack(AttackGA action)
    {
        // 기존 데미지 시스템(DealDamageGA)을 활용하여 보스에게 데미지를 입힙니다.
        List<CombatantView> targets = new() { activeBossClone };
        DealDamageGA dealDamage = new(action.Damage, targets, playerView);

        // PerformReactions 단계에 추가하여 실제 데미지 로직이 실행되게 합니다.
        ActionSystem.Instance.AddReaction(dealDamage);
        yield return new WaitForSeconds(0.1f);
    }

    // 강공격 처리
    private IEnumerator OnSuperAttack(SuperAttackGA action)
    {
        List<CombatantView> targets = new() { activeBossClone };
        DealDamageGA dealDamage = new(action.Damage, targets, playerView);
        ActionSystem.Instance.AddReaction(dealDamage);

        // 플레이어에게 '행동 불능' 혹은 '기절' 상태이상 부여
        playerView.AddStatusEffect(StatusEffectType.STUN, 1);

        yield return new WaitForSeconds(0.5f);

        // 자동으로 플레이어 턴을 종료하고 보스 턴으로 넘김
        Debug.Log("강공격 종료 - 자동으로 턴을 넘깁니다.");
    }

    // 방어 처리
    private IEnumerator OnDefense(DefenseGA action)
    {
        // 플레이어에게 방어력(Armor) 산태 이상을 추가합니다.
        // action.DefensValue는 버튼에서 설정된 방어 수치입니다.
        playerView.AddStatusEffect(StatusEffectType.ARMOR, action.DefenseValue);

        Debug.Log($"플레이어가 {action.DefenseValue}만큼 방어합니다.");

        yield return new WaitForSeconds(0.1f);
    }
}
