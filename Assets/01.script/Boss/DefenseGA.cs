using UnityEngine;

/// <summary>
/// 방어 액션 데이터를 담는 클래스입니다.
/// </summary>
public class DefenseGA : GameAction
{
    public int DefenseValue { get; private set; }

    public DefenseGA(int value)
    {
        DefenseValue = value;
    }
}
