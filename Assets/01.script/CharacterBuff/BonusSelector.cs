using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusSelector : MonoBehaviour
{
    [SerializeField] private HeroData heroData; // 인스펙터 에서 DefaultHero 에셋 연결
    [SerializeField] private string nextSceneName = "Map"; // 이동할 씬 이름

    // 최대 체력 +8 버튼에 연결할 함수
    public void SelectHealthBonus()
    {
        if(heroData != null)
        {
            heroData.AddMaxHealth(8);
            Debug.Log($"최대 체력 증가! 현재: {heroData.MaxHealth}");
            LoadNextScene();
        }
    }
    // 골드 +100 버튼에 연결할 함수
    public void SelectGoldBonus()
    {
        if(heroData != null)
        {
            heroData.gold += 100;
            Debug.Log($"골드 증가! 현재: {heroData.gold}");
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
