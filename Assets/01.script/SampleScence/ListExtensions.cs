using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// C#의 기본 List 클래스에 새로운 기능을 추가하는 확장 메서드 모음 클래스입니다.
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// 리스트 내의 임의의 요소를 '추출(Draw)' 합니다.
    /// 리스트에서 무작위로 하나를 선택해 반환하고, 해당 항목을 리스트에서 삭제합니다.
    /// </summary>
    /// <typeparam name="T">리스트에 담긴 데이터의 타입</typeparam>
    /// <param name="list">기능을 사용할 리스트 객체</param>
    /// <returns>뽑힌 요소 (리스트가 비어있으면 해당 타입의 기본값 반환)</returns>
    public static T Draw<T>(this List<T> list)
    {
        // 리스트에 뽑을 항목이 없으면 에러 방지를 위해 기본값(null 또는 0)을 반환합니다.
        if (list.Count == 0) return default;

        // 리스트 인덱스 중 하나를 무작위로 선택합니다.
        int r = Random.Range(0, list.Count);

        // 선택된 요소를 변수에 임시 저장합니다.
        T t= list[r];

        // 리스트에서 해당 요소를 제거합니다. (이게 있어야 '뽑기'가 완성됩니다.)
        list.Remove(t);

        // 뽑힌 요소를 최종적으로 반환합니다.
        return t;
    }
}
