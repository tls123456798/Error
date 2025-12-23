using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필수

public class CharacterSelectSystem : MonoBehaviour
{
    [Header("UI Groups")] // 인스펙터 에서 "UI Groups" 로 분리
    [SerializeField] private CanvasGroup buttonGroup; // 메인 타이틀 버튼
    [SerializeField] private CanvasGroup characterSelectGroup; // 캐릭터 선택 버튼
    [SerializeField] private CanvasGroup titleGroup; // 타이틀 추가

    [Header("Settings")]
    [SerializeField] private float fadeduration = 0.5f; // 전환 속도
    [SerializeField] private string gameSceneName = "CharacterBuff"; // 이동할 씬 이름

    // [Start] 버튼 클릭 시 (메인 -> 캐릭터 선택)
    public void OnStartButtonClick()
    {
        StopAllCoroutines(); // 혹시 실행 중인 페이드가 있다면 멈춤
        StartCoroutine(FadeTransition(true));
    }
    
    // [Cancel] 버튼 클릭 시 (캐릭터 선택 -> 메인)
    public void OnCancelButtonClick()
    {
        StopAllCoroutines();
        StartCoroutine(FadeTransition(false));
    }

    // 캐릭터 선택 후 [Game Start] 버튼 클릭 시 (씬 전환)
    public void OnGameStartButtonClick()
    {
        // 간단하게 바로 넘길 수도 있고, 페이드 아웃 후 넘길 수도 있습
        SceneManager.LoadScene(gameSceneName);
    }

    private IEnumerator FadeTransition(bool isGoingToSelect)
    {
        float timer = 0f;

        // 시작 전 설정
        if (isGoingToSelect) characterSelectGroup.gameObject.SetActive(true);
        else buttonGroup.gameObject.SetActive(true);
        if (titleGroup != null) titleGroup.gameObject.SetActive(true);

        while(timer < fadeduration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeduration;

            // isGoingToSelect가 true면 메인 메뉴가 사라지고 (1->0), false면 나타남(0->1)
            float mainAlph = isGoingToSelect ? Mathf.Lerp(1, 0, progress) : Mathf.Lerp(0, 1, progress);
            // 캐릭터 선택창은 반대로 작동
            float selectAlpha = isGoingToSelect ? Mathf.Lerp(0, 1, progress) : Mathf.Lerp(1, 0, progress);

            buttonGroup.alpha = mainAlph;
            if (titleGroup != null) titleGroup.alpha = mainAlph;
            characterSelectGroup.alpha = selectAlpha;

            yield return null;
        }

        // 완료 후 정리
        buttonGroup.gameObject.SetActive(!isGoingToSelect);
        if (titleGroup != null) titleGroup.gameObject.SetActive(!isGoingToSelect);
        characterSelectGroup.gameObject.SetActive(isGoingToSelect);

        // 상호작용 설정 (클릭 방지)
        buttonGroup.interactable = !isGoingToSelect;
        buttonGroup.blocksRaycasts = !isGoingToSelect;
        characterSelectGroup.interactable = isGoingToSelect;
        characterSelectGroup.blocksRaycasts = isGoingToSelect;
    }
}
