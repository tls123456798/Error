using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel; // 유니티에서 게임오버 판넬 연결

    public void Show()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    // 버튼에 연결할 함수
    public void GoToTitle()
    {
        Time.timeScale = 1; // 멈췄던 시간을 다시 돌려놓음
        SceneManager.LoadScene("Title"); // 타이틀 씬 으로 돌아감
    }
}
