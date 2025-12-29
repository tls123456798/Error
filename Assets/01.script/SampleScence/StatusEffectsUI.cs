using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// 캐릭터가 보유한 상태 이상(버프/디버프) 아이콘들을 화면에 관리하는 클래스입니다.
/// </summary>
public class StatusEffectsUI : MonoBehaviour
{
    // 상태 이상 하나를 나타낼 프리팹 (아이콘 이미지 + 중첩 텍스트 포함)
    [SerializeField] private StatusEffectUI statusEffectUIPrefab;

    // 각 상태 이상 타입에 맞는 스프라이트 에셋
    [SerializeField] private Sprite armorSprite, burnSprite;

    // 현재 화면에 표시 중인 상태 이상들을 타입별로 저장 (빠른 검색 및 업데이트용)
    private Dictionary<StatusEffectType, StatusEffectUI> statusEffectUIs = new();

    /// <summary>
    /// 상태 이상의 종류와 중첩 텍스트를 최신 정보로 갱신합니다.
    /// </summary>
    /// <param name="statusEffectType">상태 이상 종류</param>
    /// <param name="stackCount">현재 중첩 횟수 (0이면 제거)</param>
    public void UpdateStatusEffectUI(StatusEffectType statusEffectType, int stackCount)
    {
        // 중첩 횟수가 0인 경우: 해당 상태 이상 UI를 제거합니다.
        if(stackCount == 0)
        {
            if (statusEffectUIs.ContainsKey(statusEffectType))
            {
                StatusEffectUI statusEffectUI = statusEffectUIs[statusEffectType];
                statusEffectUIs.Remove(statusEffectType);
                Destroy(statusEffectUI.gameObject);

            }
        }
        // 중첩 횟수가 1 이상인 경우: UI를 생성하거나 갱신합니다.
        else
        {
            // 아직 화면에 해당 아이콘이 없다면 새로 생성합니다.
            if (!statusEffectUIs.ContainsKey(statusEffectType))
            {
                StatusEffectUI statusEffectUI = Instantiate(statusEffectUIPrefab, transform);
                statusEffectUIs.Add(statusEffectType, statusEffectUI);
            }
            // 타입에 맞는 이미지를 가져와서 아이콘과 숫자를 설정합니다.
            Sprite sprite = GetSpriteByType(statusEffectType);
            statusEffectUIs[statusEffectType].Set(sprite, stackCount);
        }
    }

    /// <summary>
    /// 상태 이상 타입에 해당하는 스프라이트를 반환합니다.
    /// </summary>
    private Sprite GetSpriteByType(StatusEffectType statusEffectType)
    {
        // switch 문을 사용하여 타입별 이미지를 매칭합니다. (C# 8.0+ 문법)
        return statusEffectType switch
        {
            StatusEffectType.ARMOR => armorSprite,
            StatusEffectType.BURN => burnSprite,
            _=> null,
        };
    }
}
