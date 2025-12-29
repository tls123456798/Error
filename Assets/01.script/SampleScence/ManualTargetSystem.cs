using UnityEngine;

/// <summary>
/// 유저가 마우스 드래그를 통해 직접 대상을 선택하는 시스템을 관리합니다.
/// 타겟팅 화살표의 시각적 연출을 제어하고, 레이캐스트를 통해 적으 판멸합니다.
/// </summary>
public class ManualTargetSystem : Singleton<ManualTargetSystem>
{
    [SerializeField] private ArrowView arrowView; // 드래그 시 나타나는 화살표 UI/이펙트 뷰
    [SerializeField] private LayerMask targetLayermask; // 적(Enemy) 레이어만 감지하기 위한 마스크 설정

    /// <summary>
    /// 타겟팅을 시작합니다. (보통 카드를 드래그하기 시작할 때 호출)
    /// </summary>
    /// <param name="startPosition">화살표가 시자고딜 월드 좌표</param>
    public void StartTargeting(Vector3 startPosition)
    {
        arrowView.gameObject.SetActive(true); // 화살표 활성화
        arrowView.SetupArrow(startPosition); // 화살표의 시작 위치 설정
    }

    /// <summary>
    /// 타겟팅을 종료하고 선택된 적이 있는지 확인합니다. (보통 드래그를 끝냈을 때 호출)
    /// </summary>
    /// <param name="endPosition">드래그가 끝난 지점의 좌표</param>
    /// <returns>객체, 선택된 대상이 없으면 null 반환</returns>
    public EnemyView EndTargeting(Vector3 endPosition)
    {
        arrowView.gameObject.SetActive(false); // 화살표 비활성화

        // 10f는 레이의 길이이며, targetLayermask를 통해 'Enemy' 레이어만 선별적으로 검사합니다.
        if(Physics.Raycast(endPosition, Vector3.forward, out RaycastHit hit, 10f,targetLayermask)
            && hit.collider != null
            && hit.transform.TryGetComponent(out EnemyView enemyView))
        {
            // 부딪힌 오브젝트에 EnemyView 컴포넌트가 있다면 해당 적을 타겟으로 반환합니다.
            return enemyView;
        }
        // 아무것도 맞지 않았거나 EnemyView가 없다면 null을 반환합니다.
        return null;
    }
}
