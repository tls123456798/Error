using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 화면에 표시되는 영웅의 시각적인 요소(이미지, 체력 바 등)를 관리하는 클래스입니다.
/// 모든 전투 단위의 공통 기능을 담은 CombatantView를 상속받습니다.
/// </summary>
public class HeroView : CombatantView
{

    // 영웅 데이터(HeroData)를 바탕으로 뷰이 초기 상태를 설정합니다.
    public void Setup(HeroData heroData)
    {
        // 부모 클래서(CombatantView)의 SetupBase를 호출하여 머리 위 HP와 이미지를 설정합니다.
        SetupBase(heroData.MaxHealth, heroData.Image);
    }

    public override void Damage(int damageAmount)
    {
        int healthBefor = CurrentHealth;

        // 부모 클래스의 기본 데미지 로직 실행 (머리 위 텍스트 갱신, 흔들림 효과 등)
        base.Damage(damageAmount);

        // 실제로 깎인 체력을 계산합니다.
        int actualDamageTaken = healthBefor - CurrentHealth;

        if (actualDamageTaken > 0)
        {
            HeroSystem.Instance.UpdateHealth(-actualDamageTaken);
        }
    }

    public void OnDamage(int damage)
    {
        Damage(damage);
    }
}
