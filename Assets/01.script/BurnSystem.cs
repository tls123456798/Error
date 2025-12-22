using System.Collections;
using UnityEngine;

/// <summary>
/// 화상(Burn) 상태 이상이 발생했을 때의 실제 동작을 처리하는 시스템
/// ActionSystem에 실행 로직(Performer)을 등록하여 화상 데미지 연출과 수치 계산을 담당
/// </summary>
public class BurnSystem : MonoBehaviour
{
    [Header("시각 효과")]
    [SerializeField] private GameObject burnVFX; // 화상 데미지 발생 시 생성될 파티클 이펙트

    private void OnEnable()
    {
        // 시스템이 활성화될 때, ActionSystem에 'ApplyBurnGA' 액션을 처리할 함수를 등록
        ActionSystem.AttachPerformer<ApplyBurnGA>(ApplyBurnPerformer);
    }
    private void OnDisable()
    {
        // 시스템이 비활성화될 때, 등록했던 실행 로직을 해제하여 메모리 누수나 잘못된 참조를 방지
        ActionSystem.DetachPerformer<ApplyBurnGA>();
    }

    /// <summary>
    /// 화상 액션(ApplyBurnGA)이 들어왔을 때 실제로 수행될 코루틴 로직
    /// </summary>
    /// <param name="applyBurnGA">액션 시스템으로부터 전달받은 화상 데이터</param>
    private IEnumerator ApplyBurnPerformer(ApplyBurnGA applyBurnGA)
    {
        // 데이터 추출: 누구에게 데미지를 줄지 타겟 정보를 가져옵니다.
        CombatantView target = applyBurnGA.Target;

        // 시각 연출: 타겟의 위치에 화상 이펙트(VFX)를 생성합니다.
        Instantiate(burnVFX, target.transform.position, Quaternion.identity);

        // 로직 실행: 타겟에게 실제 데미지를 입힙니다.
        target.Damage(applyBurnGA.BurnDamage);

        // 상태 갱신: 화상 중첩(Stack)을 1회 차감합니다.
        target.RemoveStatusEffect(StatusEffectType.BURN, 1);

        // 연출 대기: 데미지 연출을 사용자가 인지할 수 있도록 1초간 대기 시간을 가집니다.
        // 이 대기 시간 동안 ActionSystem의 전체 흐름은 멈추게(Yield)가 됩니다.
        yield return new WaitForSeconds(1f);
    }
}
