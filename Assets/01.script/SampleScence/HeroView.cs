using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 화면에 표시되는 영웅의 시각적인 요소(이미지, 체력 바 등)를 관리하는 클래스입니다.
/// 모든 전투 단위의 공통 기능을 담은 CombatantView를 상속받습니다.
/// </summary>
public class HeroView : CombatantView
{
    private bool isDying = false; // 사망 연출이 중복으로 실행되는 것을 방지

    // 영웅 데이터(HeroData)를 바탕으로 뷰이 초기 상태를 설정합니다.
    public void Setup(HeroData heroData)
    {
        // 이전 전투에서 사망했더라도 다시 나타나도록 초기화
        isDying = false;
        transform.localScale = Vector3.one;
        gameObject.SetActive(true);

        // 부모 클래서(CombatantView)의 SetupBase를 호출하여 머리 위 HP와 이미지를 설정합니다.
        SetupBase(heroData.MaxHealth, heroData.Image);
    }

    public override void Damage(int damageAmount)
    {
        if (HeroSystem.Instance.IsHeroDead() || isDying) return; 

        int healthBefor = CurrentHealth;

        // 부모 클래스의 기본 데미지 로직 실행 (머리 위 텍스트 갱신, 흔들림 효과 등)
        base.Damage(damageAmount);

        // 실제로 깎인 체력을 계산합니다.
        int actualDamageTaken = healthBefor - CurrentHealth;

        if (actualDamageTaken > 0)
        {
            HeroSystem.Instance.UpdateHealth(-actualDamageTaken);
        }

        // 체력이 0 이 되면 사망 연출 실행
        if(CurrentHealth <= 0 && !isDying)
        {
            StartCoroutine(HeroDieSequence());
        }
    }

    private IEnumerator HeroDieSequence()
    {
        isDying = true;
        Debug.Log("플레이어 사망 연출 시작");

        // 플레이어의 사망 연출 처리
        transform.DOKill();
        yield return transform.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InQuad).WaitForCompletion();

        // 연출이 끝나면 오브젝트만 비활성화 (삭제하지 않음)
        gameObject.SetActive(false);

        // 사망 후 후속 처리 (게임 오버 UI 등) 호출
        HeroSystem.Instance.HandleGameOver();
    }

    public void OnDamage(int damage)
    {
        Damage(damage);
    }
}
