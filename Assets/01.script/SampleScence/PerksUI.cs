using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 현재 플레이어가 보유한 모든 특성(Perk) UI들의 배치를 관리하는 컨테이너 클래스입니다.
/// 특성 아이콘들을 생성하거나 삭제하여 화면에 표시합니다.
/// </summary>
public class PerksUI : MonoBehaviour
{
    // 한 칸의 특성을 나타낼 프리팹 (아이콘, 툴팁 등 포함)
    [SerializeField] private PerkUI perkUIPrefab;

    // 현재 화면에 생성되어 있는 특성 UI들을 관리하는 리스트
    private readonly List<PerkUI> perkUIs = new();

    /// <summary>
    /// 새로운 특성을 화면에 추가합니다.
    /// </summary>
    /// <param name="perk">추가할 특성 데이터 객체</param>
    public void AddPerkUI(Perk perk)
    {
        // 프리팹을 현재 오브젝트(주로 가로/세로 레이아웃 그룹)의 자식으로 생성합니다.
        PerkUI perkUI = Instantiate(perkUIPrefab, transform);

        // 생성된 UI에 해당 특성 정보를 전달하여 초기화합니다.
        perkUI.Setup(perk);

        // 관리를 위해 리스트에 저장합니다.
        perkUIs.Add(perkUI);
    }

    /// <summary>
    /// 특정 특성이 제거되었을 때 해당 UI도 화면에서 삭제합니다.
    /// </summary>
    /// <param name="perk">삭제할 특성 데이터 객체</param>
    public void RemovePerkUI(Perk perk)
    {
        // LINQ를 상요하여 생성된 UI들 중, 넘겨받은 perk 데이터와 연결된 UI를 찾습니다.
        PerkUI perkUI = perkUIs.Where(pui => pui.Perk == perk).FirstOrDefault();

        // 일치하는 UI가 있다면 리스트에서 제거하고 게임 오브젝트를 파괴합니다.
        if(perkUI != null)
        {
            perkUIs.Remove(perkUI);
            Destroy(perkUI.gameObject);
        }
    }
}
