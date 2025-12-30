using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 화면에 표시되는 영웅의 시각적인 요소(이미지, 체력 바 등)를 관리하는 클래스입니다.
/// 모든 전투 단위의 공통 기능을 담은 CombatantView를 상속받습니다.
/// </summary>
public class HeroView : CombatantView
{
    [Header("UI Components")]
    [SerializeField] private Slider hpSlider; // 체력 바 (Slider)
    [SerializeField] private TextMeshPro hpText; // 체력 텍스트

    /// <summary>
    /// 영웅 데이터(HeroData)를 바탕으로 뷰의 초기 상태를 설정합니다.
    /// </summary>
    /// <param name="heroData">영웅의 초기 정보(체력, 이미지 등)를 담고 있는 데이터 에셋</param>
    public void Setup(HeroData heroData)
    {
        // 부모 클래스(CombatantView)에 정의된 SetupBase 함수를 호출하여
        // 실제 체력 수치와 스프라이트 이미지를 화면에 세팅합니다.
        SetupBase(heroData.MaxHealth, heroData.Image);

        // 현재 데이터에 맞춰 HP UI 업데이트
        UpdateHPUI(heroData.currentHealth, heroData.MaxHealth);
    }
    public void UpdateHPUI(int current, int max)
    {
        if(hpSlider != null)
        {
            hpSlider.maxValue = max;
            hpSlider.value = current;
        }
        if(hpText != null)
        {
            hpText.text = $"{current} / {max}";
        }
    }
}
