using DG.Tweening; // DOTween 라이브러리 사용 (애니메이션 처리)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내 적들의 생성, 턴 행동, 피격 및 사망 로직을 총괄하는 시스템 클래스
/// 싱글톤으로 구성되어 어디서든 접근이 가능합니다.
/// </summary>
public class EnemySystem : Singleton<EnemySystem>
{
    [SerializeField] private EnemyBoardView enemyBoardView; // 적들이 배치되는 화면(보드) 관리 뷰

    // 외부에서 적 리스트를 조회할 수 있도록 public 프로퍼티로 노출 (읽기 전용)
    public List<EnemyView> Enemies => enemyBoardView.EnemyViews;

    // 승리 패널을 제어하기 위해 추가
    [Header("Victory UI Settings")]
    [SerializeField] private GameObject victoryPanel;


    // 이벤트 연결 (이벤트 주도 설계)
    void OnEnable()
    {
        // ActionSystem에 특정 행동(GA: Game Action)이 들어오면 실행할 함수(Performer)를 연결합니다.
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer); // 적 턴 시작시
        ActionSystem.AttachPerformer<AttackHeroGA>(AttackHeroPerformer); // 적이 영웅 공격 시    
        ActionSystem.AttachPerformer<KillEnemyGA>(KillEnemyPerformer); // 적 처치 시
    }

    void OnDisable()
    {
        // 메모리 누수 방지를 위해 오브젝트가 비활성화 될 때 연결을 해제합니다.
        ActionSystem.DetachPerformer<EnemyTurnGA>();
        ActionSystem.DetachPerformer<AttackHeroGA>();
        ActionSystem.DetachPerformer<KillEnemyGA>();
    }

    /// <summary>
    /// 전달받은 데이터 리스트를 바탕으로 적들을 화면에 생성합니다.
    /// </summary>
    public void Setup(List<EnemyData> enemyDatas)
    {
        foreach (var enemyData in enemyDatas)
        {
            enemyBoardView.AddEnemy(enemyData);
        }
    }


    // Performers (실제 로직 실행부)

    /// <summary>
    /// 적들의 전체 턴을 관리합니다. (화상 데미지 체크 및 공격 명령)
    /// </summary>
    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        foreach(var enemy in enemyBoardView.EnemyViews)
        {
            // 상태 이상(화상) 체크
            int burnStacks = enemy.GetStatusEffectStacks(StatusEffectType.BURN);
            if (burnStacks > 0)
            {
                ApplyBurnGA applyBurnGA = new(burnStacks, enemy);
                ActionSystem.Instance.AddReaction(applyBurnGA); // 화상 데미지 반응 추가
            }
            // 영웅 공격 행동 예약
            AttackHeroGA attackHeroGA = new(enemy);
            ActionSystem.Instance.AddReaction(attackHeroGA);
        }
        yield return null;
    }
    /// <summary>
    /// 적이 플레이어를 공격할 때의 연출과 데미지 계산을 처리합니다.
    /// </summary>
    private IEnumerator AttackHeroPerformer(AttackHeroGA attackHeroGA)
    {
        EnemyView attacker = attackHeroGA.Attacker;

        // DOTween을 이용한 공격 애니메이션: 살짝 뒤로 갔다가(0.15초) 앞으로 돌진(0.25초)
        Tween tween = attacker.transform.DOMoveX(attacker.transform.position.x - 1f, 0.15f);
        yield return tween.WaitForCompletion(); // 애니메이션이 끝날 때까지 대기

        attacker.transform.DOMoveX(attacker.transform.position.x + 1f, 0.25f);

        // 실제 데미지를 입히는 액션을 시스템에 전달
        DealDamageGA dealDamageGA = new(attacker.AttackPower, new() { HeroSystem.Instance.HeroView }, attackHeroGA.Caster);
        ActionSystem.Instance.AddReaction(dealDamageGA);
    }
    /// <summary>
    /// 적이 죽었을 때 보드에서 제거하는 처리
    /// </summary>
    private IEnumerator KillEnemyPerformer(KillEnemyGA killEnemyGA)
    {
        // RemoveEnemy가 코루틴(IEnumerator)일 경우 yield return으로 끝날 때까지 기다림
        yield return enemyBoardView.RemoveEnemy(killEnemyGA.EnemyView);

        CheckBattleOver();
    }

    /// <summary>
    /// 실제 리스트의 개수를 확인하여 승리 판정
    /// </summary>
    public void CheckBattleOver()
    {
        // 리스트가 null이 아니고, 살아있는 적의 수가 0일 때 승리
        if(Enemies != null && Enemies.Count == 0)
        {
            OnVictory();
        }
    }

    /// <summary>
    /// 승리 패널 활성화
    /// </summary>
    private void OnVictory()
    {
        int reward = UnityEngine.Random.Range(20, 40); // 랜덤 보상
        HeroSystem.Instance.AddGold(reward); // 골드 추가

        if(victoryPanel != null)
        {
            victoryPanel.SetActive(true);

            Debug.Log("전투 승리! 보상 화면으로 이동 준비");
        }
    }
}
