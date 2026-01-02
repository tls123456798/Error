using TMPro;
using UnityEngine;

/// <summary>
/// 상단 바 UI를 관리하며, HeroSystem의 정적 이벤트를 구독하여
/// 영우으 데이터(이름, HP, Gold) 변화를 실시간으로 화면에 동기화합니다.
/// </summary>
public class TopBarUI : MonoBehaviour
{
    [Header("Data Source")]
    [SerializeField] private HeroData heroData; // 프로젝트 창의 DefaultHero 에셋 연결

    [Header("UI Text Components")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable()
    {
        // HeroSystem의 정적 이벤트를 구독합니다.
        // 데이터가 변경될 때만 UI갱신 함수가 실행됩니다.
        HeroSystem.OnHPChangedStatic += UpdateHPDisplay;
        HeroSystem.OnGoldChangedStatic += UpdateGoldDisplay;
    }
    private void OnDisable()
    {
        // 오브젝트가 비활성화될 때 이벤트 구독을 해제하여 메모리 누수를 방지합니다.
        HeroSystem.OnHPChangedStatic -= UpdateHPDisplay;
        HeroSystem.OnGoldChangedStatic -= UpdateGoldDisplay;
    }
    private void Start()
    {
        // 게임 시작 시 초기 데이터를 화면에 표시합니다.
        RefreshUI();
    }

    /// <summary>
    /// 모든 UI 요소를 현재 HeroData 기반으로 강제 새로고침합니다.
    /// </summary>
    public void RefreshUI()
    {
        if(heroData == null)
        {
            Debug.LogWarning($"{gameObject.name}의 TopBarUI: HeroData가 연결되지 않았습니다.");
            return;
        }
        // 이름 업데이트
        if(nameText != null)
        {
            nameText.text = string.IsNullOrEmpty(heroData.HeroName) ? "Unknown" : heroData.HeroName;
        }
        // 초기 수치 표시
        UpdateHPDisplay(heroData.currentHealth, heroData.MaxHealth);
        UpdateGoldDisplay(heroData.gold);
    }

    /// <summary>
    /// HP 텍스트를 업데이트합니다. (OnHPChangedStatic 이벤트 발생 시 호출)
    /// </summary>
    private void UpdateHPDisplay(int current, int max)
    {
        if(hpText != null)
        {
            hpText.text = $"{current} / {max}";
        }
    }

    /// <summary>
    /// 골드 텍스트를 업데이트합니다. (OnGoldChangedStatic 이벤트 발생 시 호출)
    /// </summary>
    private void UpdateGoldDisplay(int gold)
    {
        if(goldText != null)
        {
            goldText.text = gold.ToString();
        }
    }
}