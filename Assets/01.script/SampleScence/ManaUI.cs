using TMPro;
using UnityEngine;

/// <summary>
/// 화면 상에 마나 수치를 표시하는 역할을 담당하는 UI 클래스입니다.
/// TextMeshPro를 사용하여 텍스트를 갱신합니다.
/// </summary>
public class ManaUI : MonoBehaviour
{
    // 유니티 인스펙터에서 마나 수치를 출력할 TextMeshPro - Text 컴포넌트를 연결합니다.
    [SerializeField] private TMP_Text mana;

    /// <summary>
    /// 전달받은 현재 마나 수치를 텍스트 화면에 갱신하여 보여줍니다.
    /// MananSystem에서 마나값이 변경될 때마다 이 함수를 호출하게 됩니다.
    /// </summary>
    /// <param name="currentMana">표시할 현재 마나 값</param>
    public void UpdateManaText(int currentMana)
    {
        // 정수형인 마나 수치를 문자열(String)로 변환하여 텍스트 컴포넌트에 할당합니다.
        mana.text = currentMana.ToString();
    }
}
