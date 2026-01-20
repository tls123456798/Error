using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내 영웅(Hero)의 기초 데이터를 관리하는 데이터 컨테이너 클래스입니다.
/// ScriptableObject를 상속받아 파일 형태로 데이터를 저장할 수 있습니다.
/// </summary>
[CreateAssetMenu(menuName = "Data/Hero")] // 데이터 Creat 메뉴에 'Data/Hero' 항목을 추가합니다.
public class HeroData : ScriptableObject
{
    [Header("Visual & Basic Info")]
    [field: SerializeField] public string HeroName { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }

    // 덱 리스트 초기화 보장
    [field: SerializeField] public List<CardData> Deck {  get; private set; } = new List<CardData>();

    [Header("Stats (Permanent)")]
    [SerializeField] private int maxHealth; // 인스펙터에서 설정할 최대 체력
    public int MaxHealth => maxHealth;

    [Header("Status (Save Data)")]
    // 게임 중 실시간으로 변하는 값들입니다.
    public int currentHealth;
    public int gold;

    [Header("Death Status")]
    public bool isDead; // 플레이어의 사망 상태 추가

    /// <summary>
    /// 게임을 처음 시작할 때 데이터를 초기 상태로 되돌립니다.
    /// CharacterBuff 씬이나 타이틀 화면에서 호출하면 좋습니다.
    /// </summary>
    public void Initialize()
    {
        maxHealth = 80;
        currentHealth = maxHealth;
        gold = 0; // 초기 자금
        isDead = false; // 초기화 시 생존 상태로

        if(Deck == null) Deck = new List<CardData>();
        Debug.Log("영웅 데이터 초기화 완료)");
    }

    public void UpdateHealth(int amount)
    {
        if(isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if(currentHealth <= 0)
        {
            isDead = true;
            Debug.Log($"[HeroData] {HeroName} 사망 상태로 전환됨.");
        }
    }
  
    public void AddMaxHealth(int amount)
    {
        maxHealth += amount; // 보너스로 영구 수치 증가
        currentHealth = maxHealth; // 증가한 만큼 현재 체력도 가득 채워줌
    }

    public void RemoveCard(CardData card)
    {
        if (Deck != null && Deck.Contains(card))
        {
            Deck.Remove(card);
        }
    }

    public void AddCard(CardData card)
    {
        if(Deck != null)
        {
            Deck.Add(card);
        }
    }
}
