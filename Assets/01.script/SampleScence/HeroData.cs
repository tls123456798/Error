using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내 영웅(Hero)의 기초 데이터를 관리하는 데이터 컨테이너 클래스입니다.
/// ScriptableObject를 상속받아 파일 형태로 데이터를 저장할 수 있습니다.
/// </summary>
[CreateAssetMenu(menuName = "Data/Hero")] // 데이터 Creat 메뉴에 'Data/Hero' 항목을 추가합니다.
public class HeroData : ScriptableObject
{
    // [field: SerializeField] 방식은 변수를 private하게 보호하면서도
    // 유니티 인스펙터 창에서 값을 수정할 수 있게 해주는 깔끔한 방식입니다.

    [Header("Visual & Basic Info")]
    [field: SerializeField] public string HeroName { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public List<CardData> Deck {  get; private set; }

    [Header("Stats (Permanent)")]
    [SerializeField] private int maxHealth; // 인스펙터에서 설정할 최대 체력
    public int MaxHealth => maxHealth;

    [Header("Status (Save Data)")]
    // 게임 중 실시간으로 변하는 값들입니다.
    public int currentHealth;
    public int gold;

    /// <summary>
    /// 게임을 처음 시작할 때 데이터를 초기 상태로 되돌립니다.
    /// CharacterBuff 씬이나 타이틀 화면에서 호출하면 좋습니다.
    /// </summary>
    public void Initialize()
    {
        currentHealth = maxHealth;
        gold = 100; // 초기 자금
        Debug.Log("영웅 데이터 초기화 완료)");
    }

    public void UpdateHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
  
}
