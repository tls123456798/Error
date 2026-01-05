using UnityEngine;

public class DataInitializer : MonoBehaviour
{
    [SerializeField] private HeroData heroData;

    private void Awake()
    {
        if(heroData != null)
        {
            // 게임이 시작되자마자 데이터를 초기화합니다.
            heroData.Initialize();
        }
    }
}
