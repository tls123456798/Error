using UnityEngine;
using UnityEngine.UI;

public class BuffSceneManager : MonoBehaviour
{
    [SerializeField] private HeroData heroData; // DefaultHero 에셋 연결
    [SerializeField] private TMPro.TextMeshPro hpText;

    private void Start()
    {
        // 씬이 시작되자마자 현재 저장된 HP를 표시
        RefreshUI();
    }

    public void RefreshUI()
    {
        hpText.text = $"HP: {heroData.currentHealth}";
    }

    // hp + 8 버튼에 연결할 함수
    public void OnHealButtonClick()
    {
        heroData.UpdateHealth(8);
        RefreshUI();
    }
}
