using UnityEngine;
using UnityEngine.SceneManagement;

public class MapNode : MonoBehaviour
{
    [Header("설정")]
    public string targetSceneName; // 이 노드를 누르면 이동할 씬 이름 (예: Battle, Shop, Event)

    public void OnNodeClick()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            Debug.Log($"{targetSceneName} 씬으로 이동합니다.");
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning("이동할 씬 이름이 설정되지 않았습니다.");
        }
    }
}
