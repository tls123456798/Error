using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 게임 내에서 발생하는 모든 '행동'의 기본 단위가 되는 추상 클래스
/// 이 클래스를 상속받아 공격, 방어, 턴 시작 등의 구체적인 액션을 만듭니다.
/// </summary>
public abstract class GameAction
{
    // 액션이 실행되는 시점을 세분화하여, 특정 시점에 연쇄적으로 발생할 '반응(Reactions)' 들을 리스트로 관리합니다.

    /// <summary>
    /// 메인 액션이 실행되기 '전' 에 먼저 처리되어야 할 반응들입니다.
    /// 예: 공격 시작 전 공격력 버프 발동, 상태 이상 체크 등
    /// </summary>
    public List<GameAction> PreReactions { get; private set; } = new();

    /// <summary>
    /// 메인 액션과 '함께' 또는 메인 로직으로서 실행될 반응들입니다.
    /// 예: 실제 데미지 계산, 타격 이펙트 생성 등
    /// </summary>
    public List<GameAction> PerformReactions {  get; private set; } = new();

    /// <summary>
    /// 메인 액션이 끝난 '후' 에 이어서 발생할 반응들입니다.
    /// 예: 공격 종료 후 반격, 사망 판정, 스킬 쿨타임 시작 등
    /// </summary>
    public List<GameAction> PostReactions { get; private set; } = new();
}
