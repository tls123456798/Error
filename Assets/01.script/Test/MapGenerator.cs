using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject nodePrefab;
    public Transform contentParent;

    public GameObject linePrefab; // LinePrefab을 인스펙터에서 할당

    // 설정 값
    [SerializeField] private int mapHeight = 5; // 세로 너비 조절 가능
    [SerializeField] private int mapWidth = 2; // 가로 너비 조절 가능

    private void Start()
    {
        // 게임이 시작되자마자 맵을 생성합니다.
        GenerateMap();
    }

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
        float yOffset = -300f;
        float xOffest = 100f;
        float xSpacing = 200f;
        float ySpacing = 100f;

        foreach (var layer in nodes)
        {
            foreach (var node in layer)
            {
                // x축: 중앙 정렬 유지
                // y축: yOffset을 더해 전체 위치를 아래로 내림
                Vector2 pos = new Vector2(
                    (node.x * xSpacing - (mapWidth * xSpacing / 2f)) + xOffest,
                    node.y * ySpacing + yOffset
                    );

                GameObject obj = Instantiate(nodePrefab, contentParent);
                RectTransform rect = obj.GetComponent<RectTransform>();

                // 피벗과 앵커가 중앙으로(0.5, 0.5)인 경우 가장 잘 작동합니다.
                rect.anchoredPosition = pos;

                node.nodeObject = obj;

                TestMapNode mapNodeScript = obj.GetComponent<TestMapNode>();
                if ( mapNodeScript != null )
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
        foreach (var layer in nodes)
        {
            foreach(var node in layer)
            {
                // 각 노드가 가진 '다음 노드' 리스트를 순회
                foreach (var nextNode in node.nextNodes)
                {
                    DrawLine(node.nodeObject, nextNode.nodeObject);
                }
            }
        }
    }

    void DrawLine(GameObject from, GameObject to)
    {
        // 선 생성 및 부모 설정
        GameObject line = Instantiate(linePrefab, contentParent);
        line.transform.SetAsFirstSibling(); // 선이 노드 아이콘 뒤로 가도록 설정

        RectTransform rectFrom = from.GetComponent<RectTransform>();
        RectTransform rectTo = to.GetComponent<RectTransform>();

        // 위치 및 방향 계산
        Vector2 dir = rectTo.anchoredPosition - rectFrom. anchoredPosition;
        float distance = dir.magnitude;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 선의 속성 적용
        RectTransform lineRect = line.GetComponent<RectTransform>();
        lineRect.anchoredPosition = rectFrom.anchoredPosition; // 시작점 설정
        lineRect.sizeDelta = new Vector2(lineRect.sizeDelta.x, distance); // 길이 조절
        lineRect.rotation = Quaternion.Euler(0, 0, angle - 90f); // 각도 조절 (피벗 보정)
    }
}