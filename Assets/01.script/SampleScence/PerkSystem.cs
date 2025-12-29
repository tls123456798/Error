using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 보유한 특성(Perk)들을 관리하는 시스템 클래스입니다.
/// 특성의 추가/삭제를 제어하고 UI 및 특성 개별 로직의 실행을 동기화합니다.
/// </summary>
public class PerkSystem : Singleton<PerkSystem>
{
    // 화면에 특성 아이콘들을 표시해줄 UI 컴포넌트 참조
    [SerializeField] private PerksUI perksUI;

    // 현재 플레이어가 활성화한 특성들의 리스트
    private readonly List<Perk> perks = new();

    /// <summary>
    /// 새로운 특성을 플레이어에게 추가합니다.
    /// </summary>
    /// <param name="perk">추가할 특성 인스턴스</param>
    public void AddPerk(Perk perk)
    {
        // 내부 리스트에 특성을 추가하여 데이터를 관리합니다.
        perks.Add(perk);

        // UI 시스템에 알림을 보내 화면에 아이콘을 생성하도록 합니다.
        perksUI.AddPerkUI(perk);

        // 특성 객체 스스로가 작동(이벤트 구독 등)을 시작하도록 명령합니다.
        perk.OnAdd();
    }

    /// <summary>
    /// 보유 중인 특정 특성을 제거합니다.
    /// </summary>
    /// <param name="perk">제거할 특성 인스턴스</param>
    public void RemovePerk(Perk perk)
    {
        // 내부 리스트에서 해당 특성을 제거합니다.
        perks.Remove(perk);

        // UI 시스템에 알림을 보내 화면에서 아이콘을 삭제하도록 합니다.
        perksUI.RemovePerkUI(perk);

        // 특성 객체가 수행하던 동작(이벤트 구독 등)을 안전하게 종료하도록 합니다.
        perk.OnRemove();
    }
}
