using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleResultHandler : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel; // VictoryPanel 드래그 앤 드롭

    private void Awake()
    {
        // 시작할 때는 꺼둠
        victoryPanel.SetActive(false);
    }

    public void ShowVictoryPanel()
    {
        victoryPanel.SetActive(true);
        // 여기서 Gold: 100 같은 텍스트를 업데이트하거나
        // DOTween으로 연출을 넣으면 더 좋습니다.
    }
}
