using UnityEngine;

/// <summary>
/// 마우스 입력과 관련되 좌표 계산 기능을 제공하는 정적(static) 유틸리티 클래스입니다.
/// </summary>
public static class MouseUtil
{
    // 메인 카메라를 미리 참조해둡니다. (매번 Camera.main을 호출하는 것 보다 효율적 입니다.)
    // private static Camera camera = Camera.main;

    public static Vector3 GetMousePositionInWorldSpace(float zValue = 0f)
    {
        // 현재 활성화된 메인 카메라를 함수가 호출될 대마다 새로 가져옵니다.
        Camera currentCamera = Camera.main;

        // 카메라가 없는 예외 상황 처리
        if (currentCamera == null) return Vector3.zero;

        // 카메라가 바라보는 방향을 앞면으로 하고, 지정된 zValue 위치를 지나는 가상읜 평면(Plane)을 생성합니다.
        Plane dragPlane = new(currentCamera.transform.forward, new Vector3(0, 0, zValue));

        // 마우스의 현재 위치에서 화면 안쪽 방향으로 나가는 레이(Ray)를 생성합니다.
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);

        // 생성한 레이를 가상의 평면에 쏘아(Raycast)충돌 지점가지의 거리(distance)를 구합니다.
        if (dragPlane.Raycast(ray, out float distance))
        {
            // 레이의 시작점으로부터 구한 거리만큼 떨어진 지점의 좌표를 반환합니다.
            return ray.GetPoint(distance);
        }

        // 평면에 닿지 않은 경우 제로 벡터를 반환합니다.
        return Vector3.zero;
    }
}
