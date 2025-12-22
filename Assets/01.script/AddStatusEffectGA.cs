using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// '상태 이상 부여' 액션에 필요한 데이터를 담고 있는 데이터 클래스
/// ActionSystem에 의해 전달되어 실제 로직(Performer) 실행 시 참조
/// </summary>
public class AddStatusEffectGA : GameAction
{
    // 부여할 상태 이상의 종류 (예: 중독, 화상 등)
    public StatusEffectType StatusEffectType { get; private set; }

    // 부여할 상태 이상의 중첩 횟수
    public int StackCount {  get; private set; }

    // 상태 이상이 적용될 대상들 (전투원 리스트)
    public List<CombatantView> Targets { get; private set; }

    /// <summary>
    /// 새로운 상태 이상 부여 액션 데이터를 생성합니다.
    /// </summary>
    /// <param name="statusEffectType">상태 이상 종류</param>
    /// <param name="stackCount">중첩 수</param>
    /// <param name="targets">적용 대상 리스트</param>
    public AddStatusEffectGA(StatusEffectType statusEffectType, int stackCount, List<CombatantView> targets)
    {
        // 생성자를 통해 전달받은 데이터를 속성에 저장합니다.
        // 데이터의 오염을 박기 위해 속성(Property)은 private set으로 설정
        StatusEffectType = statusEffectType;
        StackCount = stackCount;
        Targets = targets;
    }
}
