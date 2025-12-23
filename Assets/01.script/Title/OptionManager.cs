using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UI; // UI 관련 기능을 사용하기 위해 추가

public class OptionManager : MonoBehaviour
{
    // [Serializdefield]를 사용하면 변수를 private으로 유지하면서
    // 인스펙터창에서만 드래그 앤 드롭으로 할당할 수 있어 보안과 캡슐화에 유용
    [SerializeField] private GameObject optionPanel; // inspetor에서 Panel 오브젝트를 할당할 변수

    // 버튼 클릭 시 호출될 함수
    public void OpenOptionPanel()
    {
        if (optionPanel != null)
        {
            optionPanel.SetActive(true); // 패널 활성화
        }
    }
    // 옵션창을 닫는 패널
    public void CloseOptionPanel()
    {
        if (optionPanel != null)
        {
            optionPanel.SetActive(false);
        }
    }
}
