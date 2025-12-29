using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투(Match)가 시작될 때 필요한 모든 데이터를 각 시스템에 배분하고
/// 초기 상태를 설정하는 세팅 관리 클래스입니다.
/// </summary>
public class MatchSetupSystem : MonoBehaviour
{
    [Header("초기 설정 데이터들")]
    [SerializeField] private HeroData heroData; // 플레이할 영웅의 기초 데이터
    [SerializeField] private PerkData perkData; // 시작 시 부여할 특성/강화 데이터
    [SerializeField] private List<EnemyData> enemyDatas; // 등장할 적들의 데이터 리스트

    /// <summary>
    /// 게임 오브젝트가 생성된 후 첫 번째 프레임에 실행됩니다.
    /// 모든 전투 관련 시스템들을 순서대로 초기화합니다.
    /// </summary>
    private void Start()
    {
        // 영웅 시스템 초기화: 영웅의 체력, 이밎 등을 세팅합니다.
        HeroSystem.Instance.Setup(heroData);

        // 적 시스템 초기화: 이번 전투에 등장할 적들을 생성하고 배치합니다.
        EnemySystem.Instance.Setup(enemyDatas);

        // 카드 시스템 초기화: 영웅의 덱 데이터를 기반으로 카드 덱을 구성합니다.
        CardSystem.Instance.Setup(heroData.Deck);

        // 퍽(특성) 시스템: 시작 시 기본적으로 주어지는 특성을 추가합니다.
        PerkSystem.Instance.AddPerk(new Perk(perkData));

        // 첫 턴 시작 연출: 게임 시작과 동시에 카드 5장을 뽑는 액션을 실행합니다.
        // 여기서는 AddReaction이 아닌 Perform을 사용해 즉시 첫 패를 생성합니다.
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }
}
