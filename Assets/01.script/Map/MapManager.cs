using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("진행 상태 데이터")]
    public int currentFloor = -1; // 현재 진행 중인 층 (-1은 시작 전)
    public string lastNodeID = ""; // 마지막으로 방문한 노드 이름
    public List<MapNode> allNodes = new List<MapNode>(); // 모든 노드 리스트(인스펙터에서 등록)

    private void Awake()
    {
        // 씬 이동 시에도 파괴되지 않게 설정
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
   private void OnEnable()
    {
        // 씬이 로드될 때마다 RefreshMapState가 실행되도록 이벤트를 등록합니다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 맵 씬으로 돌아올 때마다 실행됨
   private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 로드된 씬이 'Map' 씬일 때만 실행
        if (scene.name == "Map")
        {
            // 씬에 있는 새로운 노드들을 다시 찾아 리스트를 채웁니다.
            allNodes.Clear();
            allNodes.AddRange(FindObjectsOfType<MapNode>());

            // 버튼 상태 갱신
            RefreshMapState();
        }
    }

    // 맵 씬으로 다시 돌아올 때마다 버튼들의 클릭 가능 여부를 새로고침
    public void RefreshMapState()
    {
        foreach(MapNode node in allNodes)
        {
            // 처음 시작할 때는 0번 층만 클릭 가능
            if(currentFloor == -1)
            {
                node.SetInteratable(node.floorIndex == 0);
            }
            else
            {
                // 이미 방문한 노드와 연결된 '다음 노드' 만 활성화
                node.SetInteratable(IsPathValid(node));
            }
        }
    }

    private bool IsPathValid(MapNode targetNode)
    {
        // 타겟 노드가 현재 내 층 바로 위(currentFloor + 1)여야 함
        if(targetNode.floorIndex != currentFloor + 1) return false;

        // 현재 내 위치(lastNodeID)에서 갈 수 있는 경로에 있어야 함
        MapNode lastNode = allNodes.Find(n => n.nodeID == lastNodeID);
        if (lastNode != null && lastNode.nextNodes.Contains(targetNode)) return true;

        return false;
    }

    public void SelectNode(MapNode node)
    {
        currentFloor = node.floorIndex;
        lastNodeID = node.nodeID;
        SceneManager.LoadScene(node.targetSceneName);
    }
    }