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

    /// <summary>
    /// 영웅의 외형을 나타내느 스프라이트 이미지입니다.
    /// </summary>
    [field: SerializeField] public Sprite Image { get; private set; }

    /// <summary>
    /// 영웅의 기본 체력 수치 입니다.
    /// </summary>
    [field: SerializeField] public int Health {  get; private set; }

    /// <summary>
    /// 영웅이 기본적으로 소지하고 시작하는 카드들의 목록입니다.
    /// </summary>
    [field: SerializeField] public List<CardData> Deck {  get; private set; }
}
