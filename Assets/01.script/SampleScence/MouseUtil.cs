using UnityEngine;

/// <summary>
/// 마우스 입력과 관련되 좌표 계산 기능을 제공하는 정적(static) 유틸리티 클래스입니다.
/// </summary>
public static class MouseUtil
{
    // 메인 카메라를 미리 참조해둡니다. (매번 Camera.main을 호출하는 것 보다 효율적 입니다.)
    private static Camera camera = Camera.main;

    /// <summary>
    /// 마우스 화면 좌표를 게임 내 월드 좌표로 변환하여 반환합니다.
    /// 특정 Z축 평면상에서의 정확한 마우스 위치를 계산할 때 사용합니다.
    /// </summary>
    /// <param name="zValue">좌표를 계산할 월드의 Z축 깊이 값 (기본값 0)</param>
    /// <returns>변환된 월드 공간의 Vector3 좌표</returns>
    public static Vector3 GetMousePositionInWorldSpace(float zValue = 0f)
    {
        // 카메라가 바라보는 방향을 앞면으로 하고, 지정된 zValue 위치를 지나는 가상의 평면(Plane)을 생성합니다.
        Plane dragPlane = new(camera.transform.forward, new Vector3(0, 0, zValue));

        // 마우스의 현재 위치에서 화면 안쪽 방향으로 나가는 레이(Ray)를 생성합니다.
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        // 생성한 레이를 가상의 평면에 쏘아(Raycast) 충돌 지점까지의 거리(distance)를 구합니다.
        if(dragPlane.Raycast(ray, out float distance))
        {
            // 레이의 시작점으로부터 구한 거리만큼 떨어진 지점의 좌표를 반환합니다.
            return ray.GetPoint(distance);
        }
        // 평면에 닿지 않은 겨우(이론상 거의 없음) 제로 벡터를 반환합니다.
        return Vector3.zero;
    }
}
