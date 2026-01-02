using UnityEngine;

/// <summary>
/// 휴식처(Rest Site)에서 영웅의 회복 로직을 담당하는 클래스입니다.
/// </summary>
public class RestSystem : MonoBehaviour
{


    /// <summary>
    /// 휴식 버튼을 눌렀을 때 호출되는 함수입니다. (UI Button 연결용)
    /// </summary>
    public void OnRestButtonClicked()
    {
        // HeroSystem의 싱글톤 인스턴스가 존재하는지 먼저 확인합니다.
        if(HeroSystem.Instance == null)
        {
            Debug.LogError("RestSystem: 씬에 HeroSystem이 존재하지 않습니다.");
            return;
        }

        // HeroSystem을 통해 최대 체력을 가져와 회복량을 계산합니다.
        // HeroSystem에 추가한 Getmaxhealth()을 활용하거나 직접 MaxHealth에 접근합니다.
        int maxHP = HeroSystem.Instance.GetMaxHealth();

        if(maxHP <= 0)
        {
            Debug.LogWarning("RestSystem: 최대 체력이 0 이하입니다. 데이터를 확인하세요");
            return;
        }

        int healAmount = Mathf.FloorToInt(maxHP * 0.3f);

        HeroSystem.Instance.UpdateHealth(healAmount);

        // 휴식 종료 로직 실행
        EndRestSite();
    }

    private void EndRestSite()
    {
        Debug.Log("휴식처를 떠납니다.");

        // 현재 오브젝트(휴식 패널)를 비활성화 합니다.
        gameObject.SetActive(false);
    }
}
