using UnityEngine;

/// <summary>
/// 적(Enemy)이 영웅(Hero)을 공격하는 액션의 데이터를 담는 클래스
/// IHaveCaster 인터페이스를 상속받아 이 액션을 실행한 주체(Caster)가 누구이닞 명시합니다.
/// </summary>
public class AttackHeroGA : GameAction, IHaveCaster
{
    // 공격을 수행하는 적 유닛의 구체적인 정보
    public EnemyView Attacker { get; private set; }

    // IHaveCaster 인터페이스 구현: 이 액션을 일으킨 주체 (Attacker와 동일)
    // 추후 시스템에서 '누가' 공격했는지 공통된 방식으로 확인할 때 사용됩니다.
    public CombatantView Caster {  get; private set; }

    /// <summary>
    /// 적의 영웅 공격 액션 데이터를 생성합니다.
    /// </summary>
    /// <param name="attacker">공격을 시도하는 적 유닛 객체</param>
    public AttackHeroGA(EnemyView attacker)
    {
        // 공격자 정보를 저장합니다
        Attacker = attacker;

        // 인터페이스 속성인 Caster에 공격자를 할당하여
        // 시스템이 이 액션의 주인을 알 수 있게 합니다.
        Caster = Attacker;
    }
}
