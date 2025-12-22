using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 실행 중(Runtime)에 생성되어 사용되는 실제 카드 객체입니다.
/// 원본 데이터인 CardData를 참조하며, 현재 카드 상태(예: 변견된 마나 비용)를 관리합니다.
/// </summary>
public class Card
{
    // 카드의 이름 (원본 데이터의 이름을 그대로 가져옴)
    public string Title => data.name;

    // 카드의 효과 설명문
    public string Description => data.Description;

    // 카드에 표시될 이미지(일러스트)
    public Sprite Image => data.Image;

    // 플레이어가 직접 대상을 지정해야 하는 효과 (예: 적 한 명 선택 공격)
    public Effect ManualTargetEffect => data.MaualTargetEffect;

    // 카드를 낼 때 자동으로 실행되는 부가 효과 리스트 (예: 드로우, 모든 적 데미지 등)
    public List<AutoTargetEffect> OtherEffects => data.OtherEffects;

    // 현재 카드의 마나 비용 (게임 중 비용 감소 등의 로직을 위해 별도 관리
    public int Mana { get; private set; }

    // 이 카드의 기반이 되는 원본 데이터 (Readonly로 선언하여 참조 변경 방지)
    private readonly CardData data;

    /// <summary>
    /// 카드 데이터(ScriptableObject 등)를 바타응로 새로운 런타임 카드 객체를 생성합니다.
    /// </summary>
    /// <param name="cardData">카드의 기본 저옵를 담고 있는 원본 데이터</param>
    public Card(CardData cardData)
    {
        data = cardData;

        // 초기 마나 비용은 데이터의 기본값을 복사해옵니다.
        Mana = cardData.Mana;
    }
}