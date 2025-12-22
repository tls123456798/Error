using UnityEngine;

/// <summary>
/// 시작 지접부터 마우스 위치까지 이어지는 화살표 가이드라인을 그리는 클래스
/// 주로 공격 대상 지정이나 스킬 타겟팅 등의 UI/UX 연출에 사용됩니다.
/// </summary>
public class ArrowView : MonoBehaviour
{
    [Header("구성 요소")]
    [SerializeField] private GameObject arrowHead; // 화살표의 머리 부분 (끝점 이미지)
    [SerializeField] private LineRenderer lineRenderer; // 화살표의 몸통을 그릴 라인 렌더러

    private Vector3 startPosition; // 화살표가 시작되는 월드 좌표

    /// <summary>
    /// 매 프레임마다 마우스 위치에 따라 화살표의 위치와 방향을 업데이트합니다.
    /// </summary>
    private void Update()
    {
        // 현재 마우스의 월드 좌표를 가져옴
        Vector3 endPosition = MouseUtil.GetMousePositionInWorldSpace();

        // 시작접에서 화살표 머리를 향하는 방향 벡터 계산 (정규화)
        Vector3 direction = -(startPosition - arrowHead.transform.position).normalized;
         // 라인 렌더러의 끝점 설정
        lineRenderer.SetPosition(1, endPosition - direction * 0.5f);

        // 화살표 머리 위치를 마우스 좌표로 이동
        arrowHead.transform.position = endPosition;

        // 화살표 머리가 나아가는 방향을 바라보도록 회전값(Right 축) 업데이트
        arrowHead.transform.right = direction;
    }

    /// <summary>
    /// 화살표를 처음 생성하거나 초기화 할 때 호출하는 함수
    /// </summary>
    /// <param name="startPosition">화살표가 시작될 지점 (예: 캐릭터의 위치)</param>
    public void SetupArrow(Vector3 startPosition)
    {
        this.startPosition = startPosition;

        // 라인 렌더러의 시작점(0번 인덱스) 고정
        lineRenderer.SetPosition(0, startPosition);

        // 초기 끝점 위치 설정
        lineRenderer.SetPosition(1, MouseUtil.GetMousePositionInWorldSpace());
    }
}
