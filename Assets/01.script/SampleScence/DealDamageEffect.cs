using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 대상에게 직접적인 수치 피해를 입히는 'Effect' 데이터 클래스입니다.
/// 인스펙터에서 설정된 데미지 양을 바탕으로 실제 데미지 실행 액션(DealDamageGA)을 생성합니다.
/// </summary>
public class DealDamageEffect : Effect
{
    [Header("데미지 설정")]
    [Tooltip("대상에게 입힐 기본 데미지 수치입니다.")]
    [SerializeField] private int damageAmount;

    /// <summary>
    /// 설정된 데미지 수치와 런타임 정보(타겟, 시전자)를 조합하여 데미지 액션 객체를 반환합니다.
    /// </summary>
    /// <param name="targets">데미지를 입을 대상 리스트</param>
    /// <param name="caster">데미지를 입히는 주체 (공격자)</param>
    /// <returns>액션 시스템에서 실행될 실제 데미지 처리 액션</returns>
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        // 팩토리 패턴의 원리에 따라, 기획 데이터(damageAmount)와
        // 실시간 전투 정보(targets, caster)를 결합한 DealDamageGA 객체를 생성합니다.
        DealDamageGA dealDamageGA = new(damageAmount, targets,caster);

        return dealDamageGA;
    }
}
