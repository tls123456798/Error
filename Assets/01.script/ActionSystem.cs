using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임 내의 모든(GameAction)의 실행 흐름을 제어하는 시스템
/// 액션 시작 전(Pre), 실행(Perform), 종료 후(Post) 단계별로 반응 (Reaction)을 처리합니다.
/// </summary>
public class ActionSystem : Singleton<ActionSystem>
{
    // 현재 처리 중인 반응(Reaction)들의 리스트
    private List<GameAction> reactions = null;
    
    // 현재 시스템이 액션을 수행 중인지 여부 (중복 실행 방지용)
    public bool IsPerforming { get; private set; } = false;

    // 액션 타입별 사전/사후 구독자(콜백) 리스트
    private static Dictionary<Type, List<Action<GameAction>>> preSubs = new();
    private static Dictionary<Type, List<Action<GameAction>>> postSubs = new();

    // 액션 타입별 실제 실행 로직(코루틴)을 담는 저장소
    private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();

    /// <summary>
    /// 외부에서 액션을 실행하고 싶을 때 호출하는 메인 함수
    /// </summary>
    /// <param name="action">실행할 액션 객체</param>
    /// <param name="OnPerformFinished">액션이 완전히 끝났을 때 실행될 콜백</param>
    public void Perform(GameAction action, System.Action OnPerformFinished = null)
    {
        if (IsPerforming) return; // 이미 실행 중이면 중복 실행하지 않음
        IsPerforming = true;

        // Flow 코루틴을 시작하여 액션의 생명주기를 관리함
        StartCoroutine(Flow(action, () =>
        {
            IsPerforming = false;
            OnPerformFinished?.Invoke();
        }));
    }

    /// <summary>
    /// 현재 진행 중인 단계에 새로운 반응 액션을 동적으로 추가
    /// </summary>
    public void AddReaction(GameAction gameAction)
    {
        reactions?.Add(gameAction);
    }

    /// <summary>
    /// 하나의 액션이 거치는 전체 흐름(Pre -> Perform -> Post)을 제어하는 핵심 코루틴
    /// </summary>
    private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
    {
        // Pre(사전 처리) - 예: 공격전 버프 체크
        reactions = action.PreReactions;
        PerformSubscribers(action, preSubs);
        yield return PerformReactions();

        // Perform (본체 실행) - 예: 실제 데미지 계산 및 애니메이션
        reactions = action.PerformReactions;
        yield return PerformPerformer(action);
        yield return PerformReactions();

        /// Post (사후 처리) - 예: 공격 후 쿨타임 적용
        reactions = action.PostReactions;
        PerformSubscribers(action, postSubs);
        yield return PerformReactions();

        OnFlowFinished?.Invoke();
    }

    /// <summary>
    /// 등록된 Performer(실행기)를 찾아 액션 본체 로직을 실행합니다.
    /// </summary>
    private IEnumerator PerformPerformer(GameAction action)
    {
        Type type = action.GetType();
        if (performers.ContainsKey(type))
        {
            yield return performers[type](action);
        }
    }

    /// <summary>
    /// 델리게이트로 등록된 구독자(Subscriber)들에게 액션 발생을 알립니다.
    /// </summary>
    private void PerformSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs)
    {
        Type type = action.GetType();
        if (subs.ContainsKey(type))
        {
            foreach (var sub in subs[type])
            {
                sub(action);
            }

        }
    }
    /// <summary>
    /// 현재 단계에 등록된 반응(Reactions)들을 순차적으로 실행(재귀적 구조)합니다.
    /// </summary>
    private IEnumerator PerformReactions()
    {
        // 리스트를 순회하며 각 반응에 대해 다시 Flow를 태움
        foreach (var reaction in reactions)
        {
            yield return Flow(reaction);
        }
    }
    /// <summary>
    /// 특정 액션 타입에 대한 실제 실행 로직을 등록합니다.
    /// </summary>
    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);
        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);
        if (performers.ContainsKey(type)) performers[type] = wrappedPerformer;
        else performers.Add(type, wrappedPerformer);
    }
    /// <summary>
    /// 특정 액션 타입에 등록된 실행 로직을 제거합니다.
    /// </summary>
    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        if (performers.ContainsKey(type)) performers.Remove(type);
    }
    /// <summary>
    /// 특정 액션이 발생할 때 실행될 부가 효과(반응)를 구독합니다.
    /// </summary>
    /// <param name="reaction">PRE(전) 또는 POST(후) 타이밍 선택</param>
    public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        void wrappedReaction(GameAction action) => reaction((T)action);
        if (subs.ContainsKey(typeof(T)))
        {
            subs[typeof(T)].Add(wrappedReaction);
        }
        else
        {
            subs.Add(typeof(T), new());
            subs[typeof(T)].Add(wrappedReaction);
        }
    }
    /// <summary>
    /// 구독했던 반응을 해제합니다.
    /// </summary>
    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        if (subs.ContainsKey(typeof(T)))
        {
            void wrappedreaction(GameAction action) => reaction((T)action);
            subs[typeof(T)].Remove(wrappedreaction);
        }
    }

}