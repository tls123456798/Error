using UnityEngine;

[ExecuteInEditMode] // 에디터 모드에서도 선이 보이게 합니다.
public class MapLineConnector : MonoBehaviour
{
    public RectTransform startNode; // 시작 버튼
    public RectTransform endNode; // 끝 버튼
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        if(startNode != null && endNode != null)
        {
            lineRenderer.positionCount = 2;
            // UI 좌표를 월드 좌표로 변환하여 선을 긋습니다.
            lineRenderer.SetPosition(0, startNode.position);
            lineRenderer.SetPosition(1, endNode.position);
        }
    }
}
