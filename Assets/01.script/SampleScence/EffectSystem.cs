using System.Collections;
using UnityEngine;

/// <summary>
/// 기획 데이터인 'Effect' 를 실행 가능한 'GameAction' 으로 변환하여
/// 액션 시스템의 흐름(Reaction)에 추가해주는 중계 시스템입니다.
/// </summary>
public class EffectSystem : MonoBehaviour
{
    void OnEnable()
    {
        // 액션 시스템에 '효과 실행 액션(PerformEffectGA)'을 처리할 로직을 등록합니다.
        ActionSystem.AttachPerformer<PerformEffectGA>(PerformEffectPerformer);
    }
    void OnDisable()
    {
        // 시스템 비활성화 시 등록된 실행 로직을 해제합니다.
        ActionSystem.DetachPerformer<PerformEffectGA>();
    }

    /// <summary>
    /// 효과를 실제로 수행하기 위한 전 처리 로직입니다.
    /// 데이터 형태의 Effect를 실시간 명령 형태인 GameAction으로 변환합니다.
    /// </summary>
    /// <param name="performEffectGA">실행할 효과 데이터와 타겟 정보가 담긴 액션</param>
    /// <returns></returns>
    private IEnumerator PerformEffectPerformer(PerformEffectGA performEffectGA)
    {
        // Effect 데이터 객체로부터 실제 GameAction(예: DealDamageGA, DrawCardsGa 등) 을 생성합니다.
        // 이때 시전자(Caster) 정보로 HeroSystem의 HeroView를 전달합니다.
        GameAction effectAction = performEffectGA.Effect.GetGameAction(performEffectGA.Target, HeroSystem.Instance.HeroView);

        // 생성된 구체적인 액션을 ActionSystem의 반응(Reaction) 대기열에 추가합니다.
        // 이를 통해 메인 액션(카드 플레이 등) 직후에 해당 효과들이 순차적으로 실행됩니다.
        ActionSystem.Instance.AddReaction(effectAction);

        // 이 과정은 계산 및 등록 과정이므로 한 프레임 대기 후 종료합니다.
        yield return null;
    }
}
