using UnityEngine;

/// <summary>
/// 일반 공격 액션 데이터를 담는 클래서
/// </summary>
public class AttackGA : GameAction
{
    public int Damage { get; private set; }

    public AttackGA(int damage)
    {
        Damage = damage;
    }
}
