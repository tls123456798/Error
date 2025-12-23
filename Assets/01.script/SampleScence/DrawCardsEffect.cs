using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 덱에서 카드를 추가로 뽑는 효과를 정의하는 데이터 클래스
/// 인스펙터에서 설정된 숫자를 바탕으로 카드 드로우 액션(DrawCardsGA)을 생성합니다.
/// </summary>
public class DrawCardsEffect : Effect
{
    [Header("드로우 설정")]
    [Tooltip("이 효과가 발동될 때 뽑을 카드의 장수 입니다")]
    [SerializeField] private int drawAmount;

    /// <summary>
    /// 드로우 장수 데이터를 액션 시스템이 이해할 수 있는 DrawCardsGA 객체로 변환합니다.
    /// 드로우는 보통 시전자나 대상과 무관하게 플레이어의 덱에서 이루어지므로 전달받은 인자들은 사용되지 않습니다.
    /// </summary>
    /// <param name="targets">사용되지 않음 (드로우 효과의 특성)</param>
    /// <param name="caster">사용되지 않음</param>
    /// <returns>카드 시스템에서 실행된 드로우 액션</returns>
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        // 설정된 drawAmount를 담아 새로운 드로우 액션 객체를 생성하여 반환합니다.
        DrawCardsGA drawCardsGA = new(drawAmount);

        return drawCardsGA;
    }
}
