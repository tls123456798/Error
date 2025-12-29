using UnityEngine;

/// <summary>
/// '시전자(Caster)' 정보를 가지고 있어야 하는 객체들에 대한 인터페이스입니다.
/// 카드, 스킬, 상태 이상 등 "누가 이것을 사용했는가? 라는 정보가 필요한 클래스에서 상속받아 구현합니다.
/// </summary>
public interface IHaveCaster
{
    /// <summary>
    /// 해당 효과나 액션을 실행한 시전자(CombatantView)를 갸져오는 프로퍼티입니다.
    /// 인터페이스이므로 구현하는 클래스에서 이 변수를 어떻게 전달할지 정의하게 됩니다.
    /// </summary>
    CombatantView Caster { get; }
}
