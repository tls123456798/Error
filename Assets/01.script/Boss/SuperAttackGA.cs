using UnityEngine;

/// <summary>
/// 강력한 공격 액션 데이터를 담는 클래스입니다.
/// </summary>
public class SuperAttackGA : GameAction
{
    public int Damage { get; private set; }

    public SuperAttackGA(int damage)
    {
        Damage = damage;
    }
}
