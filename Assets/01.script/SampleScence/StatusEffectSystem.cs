using System.Collections;
using UnityEngine;

/// <summary>
/// 게임 내 상태 이상(Status Effect) 부여 로직을 처리하는 시스템 클래스입니다.
/// ActionSystem과 연동하여 '상태 이상 추가 액션'이 발생했을 때 이를 실제로 실행합니다.
/// </summary>
public class StatusEffectSystem : MonoBehaviour
{
    /// <summary>
    /// 오브젝트가 활성화될 때 ActionSystem에 실행기(Performer)를 등롭합니다.
    /// </summary>
    private void OnEnable()
    {
        // AddStatusEffectGA 타입의 액션이 들어오면 AddStatusEffectPerformer 함수를 실행하도록 연결합니다.
        ActionSystem.AttachPerformer<AddStatusEffectGA>(AddStatusEffectPerformer);
    }

    /// <summary>
    /// 오브젝트가 비활성화될 때 등록했던 실행기를 해제합니다. (메모리 관리 및 에러 방지)
    /// </summary>
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AddStatusEffectGA>();
    }

    /// <summary>
    /// 상태 이상을 실제로 타겟들에게 부여하는 구체적인 실행 로직입니다.
    /// 코루틴을 사용하여 한 명씩 순차적으로 적용할 수 있습니다.
    /// </summary>
    /// <param name="addStatusEffectGA">전달받은 상태 이상 추가 액션 데이터</param>
    /// <returns></returns>
    private IEnumerator AddStatusEffectPerformer(AddStatusEffectGA addStatusEffectGA)
    {
        // 액션에 포함된 모든 타겟(들)을 순회합니다.
        foreach(var target in addStatusEffectGA.Targets)
        {
            // 각 타겟(CombatantView)에게 지정된 타입의 상태 이상을 지정된 수만큼 부여합니다.
            // 이때 타겟 내부에서는 방어도가 오르거나 화상 스택이 쌓이는 등의 처리가 일어납니다.
            target.AddStatusEffect(addStatusEffectGA.StatusEffectType, addStatusEffectGA.StackCount);

            // 한 프레임을 대기합니다.
            // 만약 타겟이 많을 때 한 번에 팍! 적용되는 게 아니라 '차례대로' 적용되는 연출을 주고 싶다면
            // yield return new WaitForSeconds(0.1f); 등으로 수정할 수 있습니다.
            yield return null;
        }
    }
}
