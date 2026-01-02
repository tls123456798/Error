using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestMapNode : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public TextMeshProUGUI nodeNameText;
    public Button nodeButton;

    [Header("Node Data")]
    public string targetSceneName; // 이동할 씬 이름

    public void SetupNode(string name, Sprite icon, string sceneName)
    {
        nodeNameText.text = name;
        iconImage.sprite = icon;
        targetSceneName = sceneName;

        // 버튼 클릭 이벤트 연결
        nodeButton.onClick.RemoveAllListeners();
        nodeButton.onClick.AddListener(OnNodeClicked);
    }

    private void OnNodeClicked()
    {
        Debug.Log($"{targetSceneName}으로 이동합니다.");

        SceneManager.LoadScene(targetSceneName);
    }
}
