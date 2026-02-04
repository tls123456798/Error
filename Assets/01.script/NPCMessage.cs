using System.Collections;
using UnityEngine;

public class NPCMessage : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] private CanvasGroup messageCanvasGroup; // 말풍선의 CanvasGroup
    [SerializeField] private float fadeDuration = 0.5f; // 페이드 시간
    [SerializeField] private float displayDuration = 2.0f; // 등장 시 유지 시간

    private Coroutine fadeCoroutine;

    private void Start()
    {
        // 시작 시 투명하게 설정
        messageCanvasGroup.alpha = 0;
        // 씬 등장 시 연출 실행
        StartCoroutine(AppearSequence());
    }

    // 처음 등장할 때 연출
    private IEnumerator AppearSequence()
    {
        yield return StartCoroutine(Fade(1, fadeDuration)); // 페이드 인
        yield return new WaitForSeconds(displayDuration); // 대기
        yield return StartCoroutine(Fade(0, fadeDuration)); // 페이드 아웃
    }

    // 마우스를 올렸을 때 (Event Trigger 혹은 OnMouseEnter)
    private void OnMouseEnter()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(Fade(1, fadeDuration));
    }

    // 마우스를 뗐을 때
    private void OnMouseExit()
    {
        if(fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(Fade(0, fadeDuration));
    }

    // 페이드 로직 공통 함수
    private IEnumerator Fade(float targetAlpha, float duration)
    {
        float startAlpha = messageCanvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            messageCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }
        messageCanvasGroup.alpha = targetAlpha;
    }
}
