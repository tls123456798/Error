using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("진행 상태 데이터")]
    public static int currentFloor = -1;
    public static string lastNodeID = "";

    [Header("맵 노드 리스트")]
    public List<MapNode> allNodes = new List<MapNode>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("Map", System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log($"[MapManager] 맵 로드됨. 현재 저장된 층: {currentFloor}");
            // 씬이 새로 로드될 때마다 버튼 상태를 새로고침합니다.
            RefreshMapState();
        }
    }

    public void RefreshMapState()
    {
        Debug.Log($"현재 상태 새로고침 중... 마지막 노드: {lastNodeID}, 현재 층: {currentFloor}");
        // 씬에 새로 생성된 모든 노드들을 찾습니다.
        MapNode[] activeNodes = Object.FindObjectsByType<MapNode>(FindObjectsSortMode.None);

        foreach (MapNode node in activeNodes)
        {
            if (currentFloor == -1)
            {
                node.SetInteractable(node.floorIndex == 0);
            }
            else
            {
                node.SetInteractable(IsPathValid(node));
            }
        }
    }

    private bool IsPathValid(MapNode targetNode)
    {
        if (targetNode.floorIndex != currentFloor + 1) return false;

        // [핵심 수정] 인스펙터에 미리 등록해둔 데이터 리스트(allNodes)에서 이전 노드 정보를 찾습니다.
        MapNode lastNodeData = allNodes.Find(n => n.nodeID == lastNodeID);

        if (lastNodeData != null && lastNodeData.nextNodes != null)
        {
            // 객체 주소를 비교하는 대신, 등록된 ID 글자가 같은지 확인합니다.
            foreach (var next in lastNodeData.nextNodes)
            {
                if (next != null && next.nodeID == targetNode.nodeID)
                    return true;
            }
        }

        return false;
    }

    public void SelectNode(MapNode node)
    {
        currentFloor = node.floorIndex;
        lastNodeID = node.nodeID;

        Debug.Log($"[MapManager] 데이터 저장 됨 - 층: {currentFloor}, ID: {lastNodeID}");

        SceneManager.LoadScene(node.targetSceneName);
    }

    // 게임을 처음부터 다시 시작하고 싶을 때 호출하는 함수
    public void ResetGameProgress()
    {
        currentFloor = -1;
        lastNodeID = "";
    }
}