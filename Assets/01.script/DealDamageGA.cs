using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// '데미지 입히기' 액션의 구체적인 데이터를 담고 있는 객체입니다.
/// ActionSystem을 통해 전달되며, Damagesystem(Performer)에 의해 실제 계산이 이루어집니다.
/// IHaveCaster 인터페이스를 구현하여 누가 이 데미지를 주었는지 추적할 수 있습니다.
/// </summary>
public class DealDamageGA : GameAction, IHaveCaster
{
    // 대상에게 입힐 데미지 양
    public int Amount {  get; set; }

    // 데미지를 입을 대상 캐릭터들의 리스트
    public List<CombatantView> Targets { get; set; }

    // IHaveCaster 구현: 이 공격(액션)을 수행한 시전자
    // (예: 반격 로직이나 공격력 버프 계산 시 누가 때렸는지 확인하는 용도)
    public CombatantView Caster { get; private set; }

    /// <summary>
    /// 데미지 액션 데이터를 생성합니다.
    /// </summary>
    /// <param name="amount">데미지 수치</param>
    /// <param name="targets">피격 대상 리스트</param>
    /// <param name="caster">공격 실행자</param>
    public DealDamageGA(int amount, List<CombatantView> targets, CombatantView caster)
    {
        Amount = amount;

        // 전달받은 타겟 리스트를 그대로 참조하지 않고 새로운 리스트로 복사하여 저장합니다.
        // 이는 외부에서 원본 리스트를 수정해도 이 액션의 타겟 정보가 변하지 않도록 보호하기 위합입니다.
        Targets = new(targets);

        Caster = caster;
    }
}
