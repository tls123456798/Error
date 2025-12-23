using TMPro;
using UnityEngine;

/// <summary>
/// 화면에 보이는 적 캐릭터의 외형과 UI(공격력 등)를 제어하는 클래스
/// 공통 전투 유닛 기능인 CombatantView를 상속받습니다.
/// </summary>
public class EnemyView : CombatantView
{
    [SerializeField] private TMP_Text attackText; // 공격력 수치를 화면에 표시할 텍스트 컴포넌트

    // 이 적의 현재 공격력 수치 (외부 시스템인 EnemySystem 등에서 참조 가능)
    public int AttackPower { get; set; }

    /// <summary>
    /// EnemyData(ScriptableObject)를 전달받아 적의 초기 외형과 능력치를 설정합니다.
    /// </summary>
    /// <param name="enemyData">설정할 적의 원본 데이터</param>
    public void Setup(EnemyData enemyData)
    {
        // 공격력 데이터 할당
        AttackPower = enemyData.AttackPower;

        // 공격력 텍스트 UI 업데이트
        UpdateAttackText();

        // 부모 클래스(CombatantView)의 설정 호출 (체력, 이미지 등 공통 요소 처리)
        // 보통 부모 클래스에 protected로 선언된 설정 함수가 있을 때 이처럼 사용합니다.
        SetupBase(enemyData.Health, enemyData.Image);
    }

    /// <summary>
    /// 현재 공격력 수치를 UI 텍스트에 갱신하여 보여줍니다.
    /// </summary>
    private void UpdateAttackText()
    {
        // "ATK:10"과 같은 형식으로 텍스트를 구성합니다.
        attackText.text = "ATK:" + AttackPower;
    }
}
