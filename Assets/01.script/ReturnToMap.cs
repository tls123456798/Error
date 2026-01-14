using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMap : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private string mapSceneName = "Map"; // 이동한 씬의 이름

    /// <summary>
    /// 버튼의 OnClick 이벤트에 이 함수를 연결
    /// </summary>
    public void BackToMap()
    {
        // 맵 씬으로 이동
        SceneManager.LoadScene(mapSceneName);
        StartCoroutine(RefreshMapNextFrame());
    }

    private System.Collections.IEnumerator RefreshMapNextFrame()
    {
        // 한 프레임 대기
        yield return null;

        if(MapManager.Instance != null)
        {
            MapManager.Instance.RefreshMapState();
        }
    }
}
