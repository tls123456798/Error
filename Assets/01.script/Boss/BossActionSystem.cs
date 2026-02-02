using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActionSystem : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private CombatantView playerView; // 플레이어 컴뱃턴트
    [SerializeField] private CombatantView bossView; // 보스 컴뱃턴트

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
        List<CombatantView> targets = new() { bossView };
        DealDamageGA dealDamage = new(action.Damage, targets, playerView);

        // PerformReactions 단계에 추가하여 실제 데미지 로직이 실행되게 합니다.
        ActionSystem.Instance.AddReaction(dealDamage);
        yield return new WaitForSeconds(0.1f);
    }

    // 강공격 처리
    private IEnumerator OnSuperAttack(SuperAttackGA action)
    {
        List<CombatantView> targets = new() { bossView };
        DealDamageGA dealDamage = new(action.Damage, targets, playerView);

        ActionSystem.Instance.AddReaction(dealDamage);
        yield return new WaitForSeconds(0.2f);
    }

    // 방어 처리
    private IEnumerator OnDefense(DefenseGA action)
    {
        // TODO: 방어는 데미지를 깎는 것이 아닌 플레이어의 상태(방어막 등)를 변경합니다.
        // 현재는 로그만 출력하거나, 플레이어에게 '방어' 버프 액션을 추가할 수 있습니다.
        Debug.Log($"플레이어가 {action.DefenseValue}만큼 방어합니다.");
        yield return new WaitForSeconds(0.1f);
    }
}
