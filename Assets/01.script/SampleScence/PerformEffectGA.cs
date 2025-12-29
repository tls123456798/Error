using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 특적 효과(Effect)를 지정된 대상들(Targets)에게 실행하도록 지시하는 게임 액션 클래스입니다.
/// 카드 사용, 유물 효과, 적의 스킬 등 모든 '효과 발동'의 데이터를 담는 컨테이너 역할을 합니다.
/// </summary>
public class PerformEffectGA : GameAction
{
    /// <summary>
    /// 실행될 구체적인 효과 데이터(예: 데미지 수치, 상태 이상 종류 등) 입니다.
    /// </summary>
    public Effect Effect { get; set; }

    /// <summary>
    /// 효과가 적용될 대상 리스트입니다.
    /// </summary>
    public List<CombatantView> Target {  get; set; }

    /// <summary>
    /// PerformEffectGA를 생성할 때 효과와 대상을 설정합니다.
    /// </summary>
    /// <param name="effect">적용할 효과 객체</param>
    /// <param name="targets">효과를 받을 대상들 (null일 수 있음)</param>
    public PerformEffectGA(Effect effect,List<CombatantView> targets)
    {
        Effect = effect;

        // 타겟 리스트가 null인지 확인하고, null이 아니라면 원본 리스트를 복사하여 새 리스트를 만듭니다
        // 이는 원본 리스트가 외부에서 수정되어도 이 액션의 타겟 정보가 변하지 않도록 보호하는 기법입니다.
        Target = targets==null ? null : new(targets);
    }
}
