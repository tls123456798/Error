using UnityEngine;

public class GameDateManager : MonoBehaviour
{
    public static GameDateManager Instance; // 어디서든 접근 가능하게 만듭니다. (싱글톤)

    public BonusItem selectedBonus; // 선택한 아이템 정보를 저장할 칸

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 다음 씬으로 넘어가도 파괴 안 됨
        }
        else
        {
            Destroy(gameObject); // 중복 생성 방지
        }
    }
}
