using System.Collections.Generic;

/// <summary>
/// 게임 내의 모든 효과(데미지, 버프, 드로우 등)의 최상위 추상 클래스 입니다.
/// '무엇을 할 것인가' 에 대한 기획 데이터를 실제 실행 가능한 ' GameAction' 으로 변환하는 인터페이스를 제공합니다.
/// </summary>

[System.Serializable]

public abstract class Effect
{
    /// <summary>
    /// 전달받은 타겟과 시전자 정보를 바탕으로, 해당 효과를 수행하기 위한 구체적인 GameAction 객체를 생성하여 반환합니다.
    /// </summary>
    /// <param name="targets">효과가 적용될 대상 리스트</param>
    /// <param name="caster">효과를 실행하는 주체 (캐릭터)</param>
    /// <returns>액션 시스템(ActionSystem)에서 실행될 실제 액션 객체</returns>
    public abstract GameAction GetGameAction(List<CombatantView> targets, CombatantView caster);
}
