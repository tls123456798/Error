using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthBonus : MonoBehaviour
{
    [SerializeField] private HeroData heroData; // 플레이어 데이터 연결
    [SerializeField] private string nextSceneName = "Map"; // 이동한 씬 이름

    // 임시 선택지: 체력을 +8 버튼에 연결할 함수
    public void HPPlus()
    {
        if(heroData != null)
        {
            heroData.AddMaxHealth(8);
            Debug.Log($"최대 체력 증가! 현재: {heroData.MaxHealth}");
            LoadNextScene();
        }
    }

    // 임시 선택지: 체력을 -8버튼에 연결할 함수
    public void HPMinus()
    {
        if(heroData != null)
        {
            heroData.AddMaxHealth(-8);
            Debug.Log($"최대 체력 감소! 현재: {heroData.MaxHealth}");
            LoadNextScene();
        }
    }

    // 다음 씬(Map)으로 이동
    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
