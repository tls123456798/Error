using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject nodePrefab;
    public Transform contentParent;

    // 설정 값
    [SerializeField] private int mapHeight = 10; // 총 10층
    [SerializeField] private int mapWidth = 5; // 가로 너비 (0~4 칸)

    // 게임 시작 시 혹은 버튼 이벤트로 호출
    public void GenerateMap()
    {
        // 기존에 생성된 노드가 있다면 제거 (초기화)
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // 노드 데이터 생성 루프
        List<List<NodeInfo>> allNodes = new List<List<NodeInfo>>();

        for (int y = 0; y < mapHeight; y++)
        {
            List<NodeInfo> currentLayer = new List<NodeInfo>();
            int nodeCount = Random.Range(3, 6); // 한 층에 3~5개 노드 생성

            for(int i = 0; i < nodeCount; i++)
            {
                NodeInfo node = new NodeInfo();
                node.y = y;
                node.x = Random.Range(0, mapWidth);

                // 노드 타입 결정 로직 적용
                node.nodeType = GetRandomType(y);
                currentLayer.Add(node);
            }
            allNodes.Add(currentLayer);
        }

        // 연결 로직 (위층 노드와 연결)
        for(int y = 0; y < mapHeight - 1; y++)
        {
            foreach (var node in allNodes[y])
            {
                foreach(var nextNode in allNodes[y + 1])
                {
                    // 가로 거리가 1칸 이내인 노드들끼리만 연결
                    if (Mathf.Abs(node.x - nextNode.x) <= 1)
                    {
                        node.nextNodes.Add(nextNode);
                    }
                }
            }
        }

        // 시각화 (실제 UI 생성)
        DrawMap(allNodes);
    }

    // 노드 타입 결정 함수
    string GetRandomType(int y)
    {
        if (y == 0) return "Battle"; // 시작 층
        if (y == mapHeight - 1) return "Boss"; // 마지막 층

        float rand = Random.value;
        if (rand < 0.15f) return "Rest"; // 15% 휴식
        if (rand < 0.25f) return "Shop"; // 10% 상점
        if (rand < 0.40f) return "Elite"; // 엘리트 전투
        return "Battle";
    }

    // 시각화 및 선 그리기 준비
    void DrawMap(List<List<NodeInfo>> nodes)
    {
        foreach (var layer in nodes)
        {
            foreach (var node in layer)
            {
                // UI 좌표 계산 (x축 주앙 정렬 보정 포함)
                // 150과 250은 노드 간의 간격입니다. 인스펙터 디자인에 맞춰 조절하세요.
                Vector2 pos = new Vector2(node.x * 150 - (mapWidth * 150 / 2f), node.y * 250);

                GameObject obj = Instantiate(nodePrefab, contentParent);
                RectTransform rect = obj.GetComponent<RectTransform>();
                rect.anchoredPosition = pos;

                node.nodeObject = obj; // 생성된 객체를 데이터에 저장

                // MapNode.cs의 Setup 합수 호출 (아이콘 및 이름 설정)
                TestMapNode mapNodeScript = obj.GetComponent <TestMapNode>();
                if(mapNodeScript != null)
                {
                    mapNodeScript.SetupNode(node.nodeType, null, node.nodeType);
                }
            }
        }

        // 선 그리는 함수 호출
        CreateLines(nodes);
    }

    void CreateLines(List<List<NodeInfo>> nodes)
    {
        Debug.Log("모든 노드 생성 완료. 이제 노드 사이의 연결선을 그릴 차례입니다.");
        // 여기에 LineRenderer 또는 Image를 이용한 선 생성 코드가 들어갑니다.
    }
}