using UnityEngine;

public class SceneBGM : MonoBehaviour
{
    // BGM 그룹을 구분하기 위한 ID (예: "GroupA", "GroupB")
    public string bgmGroupID;
    private static SceneBGM currentInstance;

    void Awake()
    {
        // 1. 현재 재생 중인 BGM이 있는지 확인
        if (currentInstance != null)
        {
            // 재생 중인 BGM과 내 ID가 같다면? (같은 그룹의 음악이 이미 나오고 있음)
            if (currentInstance.bgmGroupID == this.bgmGroupID)
            {
                Destroy(gameObject); // 새로 생긴 나는 파괴 (기존 음악 유지)
                return;
            }
            else
            {
                // 다른 그룹의 음악이 나오고 있다면? (음악 교체 시점)
                Destroy(currentInstance.gameObject); // 기존 음악 파괴
            }
        }

        // 2. 내 음악을 현재 인스턴스로 등록
        currentInstance = this;
        transform.SetParent(null); // 최상위로 이동
        DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 방지
    }
}