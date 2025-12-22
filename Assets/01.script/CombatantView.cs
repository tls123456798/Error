using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 전투에 참여하는 모든 캐릭터(영웅, 적)의 공통적인 상태와 외형을 관리합니다.
/// 체력, 방어력, 상태 이상 계산 및 UI 업데이트를 담당하는 핵심 클래스입니다.
/// </summary>
public class CombatantView : MonoBehaviour
{
    [Header("시각적 UI 요소")]
    [SerializeField] private TMP_Text healthText; // 체력 표시 텍스트
    [SerializeField] private SpriteRenderer spriteRenderer; // 캐릭터 외형 이미지
    [SerializeField] private StatusEffectsUI statusEffectsUI; // 상태 이상 아이콘 관리 UI

    // 현재 생존 상태 데이터
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    // 상태 이상 종류와 중첩(Stack) 횟수를 저장하는 저장소
    private Dictionary<StatusEffectType, int> statusEffects = new();

    /// <summary>
    /// 캐릭터의 기본 정보를 설정합니다. (자식 클래스인 HeroView, EnemyView 등에서 호출)
    /// </summary>
    protected void SetupBase(int health, Sprite image)
    {
        MaxHealth = CurrentHealth = health;
        spriteRenderer.sprite = image;
        UpdateHealthText();
    }

    /// <summary>
    /// 체력 UI를 최신 정보로 갱신합니다.
    /// </summary>
    private void UpdateHealthText()
    {
        healthText.text = "HP:" + CurrentHealth;
    }

    /// <summary>
    /// 캐릭터가 피해를 입었을 때 호출됩니다. 바어력 계산 로직을 포함합니다.
    /// </summary>
    /// <param name="damageAmount">입힐 데미지 수치</param>
    public void Damage(int damageAmount)
    {
        int remainingDamage = damageAmount;
        int currentArmor = GetStatusEffectStacks(StatusEffectType.ARMOR);

        // 바어력(Amor)이 있다면 데미지를 먼저 흡수함
        if (currentArmor > 0)
        {
            if(currentArmor >= damageAmount)
            {
                // 방어력이 데미지보다 크거나 같으면 바어력만 깎고 피해는 0
                RemoveStatusEffect(StatusEffectType.ARMOR, remainingDamage);
                remainingDamage = 0;
            }
            else if (currentArmor < damageAmount)
            {
                // 방어력이 부족하면 방어력을 모두 소진하고 남은 데미지를 계산
                RemoveStatusEffect(StatusEffectType.ARMOR, currentArmor);
                remainingDamage -= currentArmor;
            }
        }

        // 남은 데미자가 있다면 체력에서 차감
        if (remainingDamage > 0)
        {
            CurrentHealth -= damageAmount; // (Tip: 기존 코드의 damageAmount 대신 remainingDamage를 쓰는 것이 맞습니다)
            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }
        }

        // 피격 연출: 캐릭터를 흔들리게 함 (DoTween)
        transform.DOShakePosition(0.2f, 0.5f);
        UpdateHealthText();
    }

    /// <summary>
    /// 새로운 상태 이상을 추가하거나 기존 중첩 횟수를 늘립니다.
    /// </summary>
    public void AddStatusEffect(StatusEffectType type, int stackCount)
    {
        if (statusEffects.ContainsKey(type))
        {
            statusEffects[type] += stackCount;
        }
        else
        {
            statusEffects.Add(type, stackCount);
        }
        // UI에 상태 이상 변화를 알림
        statusEffectsUI.UpdateStatusEffectUI(type, GetStatusEffectStacks(type));
    }

    /// <summary>
    /// 상태 이상 중첩 횟수를 줄입니다. 0 이하가 되면 저장소에서 제거합니다.
    /// </summary>
    public void RemoveStatusEffect(StatusEffectType type, int stackCount)
    {
        if (statusEffects.ContainsKey(type))
        {
            statusEffects[type] -= stackCount;
            if (statusEffects[type] <= 0)
            {
                statusEffects.Remove(type);
            }
        }
        // UI 갱신
        statusEffectsUI.UpdateStatusEffectUI(type, GetStatusEffectStacks(type));
    }

    /// <summary>
    /// 특정 상태 이상이 현재 몇 중첩인지 반환합니다.
    /// </summary>
    public int GetStatusEffectStacks(StatusEffectType type)
    {
         if (statusEffects.ContainsKey(type)) return statusEffects[type];
         else return 0;
    }
}
