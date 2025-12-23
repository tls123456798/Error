using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 전투 화면에서 적들이 배치되는 구역(보드)을 관리하는 클래스입니다.
/// 적의 생성 위치(Slot)를 관리하고, 적의 추가 및 제거 시의 시각적 처리르 담당합니다.
/// </summary>
public class EnemyBoardView : MonoBehaviour
{
    [Header("배치 설정")]
    [SerializeField] private List<Transform> slots; // 적들이 소환될 위치값들을 담고 있는 리스트

    /// <summary>
    /// 현재 보드에 존재하는 적(EnemyView)들의 리스트이빈다.
    /// </summary>
    public List<EnemyView> EnemyViews { get; private set; } = new();

    /// <summary>
    /// 새로운 적을 보드의 빈 슬롯에 생성하고 등록합니다.
    /// </summary>
    /// <param name="enemyData">생성할 적의 기본 데이터(체력, 이미지 등)</param>
    public void AddEnemy(EnemyData enemyData)
    {
        // 현재 적의 숫자를 인덱스로 사용하여 다음 빈 슬롯을 선택합니다.
        Transform slot = slots[EnemyViews.Count];

        // EnemyViewCreator를 통해 실제 적 오브젝트를 생성합니다.
        EnemyView enemyView = EnemyViewCreator.Instance.CreateEnemyView(enemyData, slot.position, slot.rotation);

        // 생성된 적을 해당 슬롯의 자식으로 설정하여 위치를 고정시킵니다.
        enemyView.transform.parent = slot;

        // 관리 리스트에 추가합니다.
        EnemyViews.Add(enemyView);
    }

    /// <summary>
    /// 특정 적을 보드에서 제거하고 파괴 연출을 실행하는 코루틴 입니다.
    /// </summary>
    /// <param name="enemyView">제거할 적의 뷰 객체</param>
    public IEnumerator RemoveEnemy(EnemyView enemyView)
    {
        // 관리 리스트에서 해당 적을 먼저 제외합니다.
        EnemyViews.Remove(enemyView);

        // 시각 연출: DoTween을 사용하여 0.25초 동안 크기를 0으로 줄입니다.
        Tween tween = enemyView.transform.DOScale(Vector3.zero, 0.25f);

        // 연출이 끝날 때까지 대기합니다.
        yield return tween.WaitForCompletion();

        // 연출이 끝나면 실제 게임 오브젝트를 파괴합니다.
        Destroy(enemyView.gameObject);
    }
}
