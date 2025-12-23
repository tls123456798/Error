using System.Collections;
using UnityEngine;

/// <summary>
/// 게임 내에서 발생하는 실제 데미지 연출고 체력 계산 로직을 수행하는 시스템입니다.
/// 데미지 처리 후 사망 조건(HP <= 0)을 체크하여 후속 액션을 발생시킵니다.
/// </summary>
public class DamageSystem : MonoBehaviour
{
    [Header("시각 효과")]
    [SerializeField] private GameObject damageVFX; // 데미지를 입었을 때 나타날 파티클 이펙트

    void OnEnable()
    {
        // 액션 시스템에 데미지 실행 로직(Performer)을 등록합니다.
        ActionSystem.AttachPerformer<DealDamageGA>(DealdamagePerformer);
    }
    void OnDisable()
    {
        // 시스템 종료 시 등록했던 로직을 안전하게 해제합니다.
        ActionSystem.DetachPerformer<DealDamageGA>();
    }

    /// <summary>
    /// 데미지 액션(DealDamageGA)이 들어왔을 때 실행되는 실제 코루틴 로직입니다.
    /// </summary>
    /// <param name="dealDamageGA">액션 시스템으로부터 전달받은 타겟 및 데미지 수치 데이터</param>
    private IEnumerator DealdamagePerformer(DealDamageGA dealDamageGA)
    {
        // 타겟 리스트를 순회하면 순차적으로 데미지를 입힙니다. (광역 공격 대응 가능)
        foreach(var target in dealDamageGA.Targets)
        {
            // 실제 데미지 수치 적용 (CombatantView 내부 로직 호출)
            target.Damage(dealDamageGA.Amount);

            // 피격 이펙트 생성
            if(damageVFX != null)
            {
                Instantiate(damageVFX, target.transform.position, Quaternion.identity);
            }
            
            // 타격 간의 짧은 간격을 주어 연출 효과를 높임
            yield return new WaitForSeconds(0.15f);

            // 사망 판정 체크
            if (target.CurrentHealth <= 0)
            {
                // 타겟이 적(EnemyView)인 경우
                if(target is EnemyView enemyView)
                {
                    // 사망 액션을 생성하여 반응(Reaction) 리스트에 추가
                    KillEnemyGA killEnemyGA = new(enemyView);
                    ActionSystem.Instance.AddReaction(killEnemyGA);
                }
                // 타겟이 영웅(플레이어)인 경우
                else
                {
                    // TODO: 게임 오버 씬 전환 또는 결과창 출력 로직이 들어갈 자리입니다.
                    Debug.Log("Game Over Logic Needed");
                }
            }
        }
    }
}
