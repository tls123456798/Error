using UnityEngine;
using UnityEngine.SceneManagement;

public class NextMap : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Map"; // 이동할 씬 이름

    public void OnClickSelect()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
