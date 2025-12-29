using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 화면에 표시되는 개별 상태 이상(버프/디버프) 아이콘의 시각적 요소를 제어합니다.
/// 아이콘 이미지와 중첩 횟수(Stack)를 텍스트로 나타냅니다.
/// </summary>
public class StatusEffectUI : MonoBehaviour
{
    // 유니티 인스펙터에서 할당할 상태 이상 아이콘 이미지 컴포넌트
    [SerializeField] private Image image;

    // 중첩 횟수를 표시할 TextMeshPro 컴포넌트
    [SerializeField] private TMP_Text stackCountText;

    /// <summary>
    /// 상태 이상 아이콘 이미지와 중첩 숫자를 설정합니다.
    /// StatusEffectsUI 관리자에 의해 실시간으로 호출됩니다.
    /// </summary>
    /// <param name="sprite">표시할 상태 이상 스프라이트 (방어도, 화상 등)</param>
    /// <param name="stackCount">현재 쌓여있는 중첩 수</param>
    public void Set(Sprite sprite, int stackCount)
    {
        // 전달받은 이미지를 이미지 컴포넌트에 할당합니다.
        image.sprite = sprite;

        // 숫자를 문자열로 변환하여 텍스트 컴포넌트에 할당합니다.
        // (예: 5 -> "5")
        stackCountText.text = stackCount.ToString();
    }
}
