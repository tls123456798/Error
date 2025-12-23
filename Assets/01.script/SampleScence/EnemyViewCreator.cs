using UnityEngine;

/// <summary>
/// 적(EnemyView) 캐릭터를 실제로 게임 세상에 생성하는 역할을 당담하는 클래스입니다.
/// 생성과 동시에 초기 설정(Setup)까지 완료 하여 반환해주는 '공장' 역할을 수행합니다.
/// </summary>
public class EnemyViewCreator : Singleton<EnemyViewCreator>
{
    // 생성할 적의 원본 프리팹 (인스펙터 창에서 할당)
    [SerializeField] private EnemyView enemyViewPrefab;

    /// <summary>
    /// 새로운 적 오브젝트를 생성하고 데이터를 초기화 합니다.
    /// </summary>
    /// <param name="enemyData">적에게 부여할 능력치 데이터</param>
    /// <param name="position">생성될 위치</param>
    /// <param name="rotation">생성될 회전값</param>
    /// <returns>생성 및 설정이 완료된</returns>
    public EnemyView CreateEnemyView(EnemyData enemyData, Vector3 position, Quaternion rotation)
    {
        // 프리팹을 지정된 위치와 회전값으로 복제(Instantiate)합니다.
        EnemyView enemyView = Instantiate(enemyViewPrefab, position, rotation);

        // 복제된 적에게 EnemyData를 전달하여 이름, 체력, 공격력 등을 설정하게 합니다.
        enemyView.Setup(enemyData);

        // 모든 준비가 끝난 적 객체를 결과물로 돌려줍니다.
        return enemyView;
    }
}
