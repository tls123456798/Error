using TMPro;
using UnityEngine;

public class TopBarUI : MonoBehaviour
{
    [SerializeField] private HeroData heroData; // DefaultHero 연결

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start()
    {
        RefreshUI();
    }

    // 데이터가 바뀔 때마다 호출하여 UI 갱신
    public void RefreshUI()
    {
        // 이름 설정
        if(nameText != null)
        {
            nameText.text = heroData.HeroName;
        }

        // HP 설정
        if(hpText != null)
        {
            hpText.text = $"{heroData.currentHealth} / {heroData.MaxHealth}";
        }

        // 골드 설정
        if(goldText != null)
        {
            goldText.text = heroData.gold.ToString();
        }
    }
}
