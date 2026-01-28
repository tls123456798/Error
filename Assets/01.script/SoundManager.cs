using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 전체의 사운드를 관리하며, 씬이 전환되어도 파괴되지 않습니다.
/// </summary>
public class SoundManager : MonoBehaviour
{
    // 외부에서 접근 가능하도록 싱글톤 설정
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource; // 배경음용 오디오 소스

    [Header("UI References")]
    [SerializeField] private Slider bgmSlider; // 볼륨 조절 슬라이더

    private void Awake()
    {
        // --- 싱글톤 및 파괴 방지 로직 ---
        if (Instance == null)
        {
            Instance = this;
            // 최상위 오브젝트여야 DontDestroyOnLoad가 작동합니다.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 다른 씬에서 넘어온 매니저가 있다면 새로 생성된 것은 파괴
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // 초기 볼륨 설정 및 슬라이더 연결
        SetupSlider();
    }

    /// <summary>
    /// 슬라이더를 찾아 현재 볼륨을 동기화하고 이벤트를 연결합니다.
    /// 씬이 전환된 후 새로운 슬라이더를 연결할 때도 사용할 수 있습니다.
    /// </summary>
    public void SetupSlider()
    {
        if (bgmSlider != null && bgmSource != null)
        {
            bgmSlider.value = bgmSource.volume;
            // 기존 리스너를 한 번 제거하고 새로 등록 (중복 방지)
            bgmSlider.onValueChanged.RemoveAllListeners();
            bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        }
    }

    /// <summary>
    /// 새로운 배경음악으로 교체합니다.
    /// </summary>
    /// <param name="newClip">바꿀 오디오 클립</param>
    /// <param name="loop">반복 재생 여부</param>
    public void ChangeBGM(AudioClip newClip, bool loop = true)
    {
        if (newClip == null || bgmSource.clip == newClip) return;

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    /// <summary>
    /// 슬라이더 값에 따라 볼륨을 조절합니다.
    /// </summary>
    public void OnBgmVolumeChanged(float value)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = value;
        }
    }

    // 현재 볼륨 수치를 반환 (다른 스크립트에서 참조용)
    public float GetVolume()
    {
        return bgmSource != null ? bgmSource.volume : 0.5f;
    }
}