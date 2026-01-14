using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    [Header("설정")]
    public string nodeID; // 노드 고유 이름 (예: Node_1_1)
    public int floorIndex; // 이 노드가 위치한 층 (0부터 시작)
    public string targetSceneName; // 이 노드를 누르면 이동할 씬 이름 (예: Battle, Shop, Event)

    [Header("연결된 다음 노듣들")]
    // 슬레이더 스파이어 처럼 선이 이어지느 다음 노드들을 인스펙에 할당
    public List<MapNode> nextNodes;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    // 매니저가 버튼을 켜고 끌 때 사용
    public void SetInteratable(bool state)
    {
        if(button != null) button.interactable = state;
    }

    public void OnNodeClick()
    {
        // 직접 이동하지 않고 매니저에게 판단을 맡깁니다.
        if(MapManager.Instance != null)
        {
            MapManager.Instance.SelectNode(this);
        }
    }
}
