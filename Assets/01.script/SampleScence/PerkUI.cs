using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 화면에 표시되는 개별 특성(Perk) 아이콘의 시각적 요소를 관리하는 클래스입니다.
/// </summary>
public class PerkUI : MonoBehaviour
{
    // 유니티 인스펙터에서 할당한 아이콘 표시용 이미지 컴포넌트
    [SerializeField] private Image image;

    /// <summary>
    /// 현재 이 UI가 나타내고 있는 특성(Perk) 데이터 객체입니다.
    /// 외부(PerksUI 등)에서 이 UI가 어떤 특성인지 확인할 때 사용합니다.
    /// </summary>
    public Perk Perk {  get; private set; }

    /// <summary>
    /// 전달받은 특성 데이터를 바탕으로 UI의 정보를 초기화합니다.
    /// </summary>
    /// <param name="perk">표시할 특성 데이터</param>
    public void Setup(Perk perk)
    {
        // 현재 UI가 담당할 특성 데이터를 저장합니다.
        Perk = perk;

        // Perk 데이터에 저장된 스프라이트 이미지를 UI 이미지 컴포넌트에 적용합니다.
        image.sprite = perk.Image;
    }
}
