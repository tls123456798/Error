using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필수

[System.Serializable]
public struct CharacterInfo
{
    public string charName; // 캐릭터 이름
    [TextArea] public string description; // 캐릭터 설명
    public Sprite charSprite; // 캐릭터 배경 일러스트
}

public class CharacterSelectSystem : MonoBehaviour
{
    [Header("UI Groups")] // 인스펙터 에서 "UI Groups" 로 분리
    [SerializeField] private CanvasGroup buttonGroup; // 메인 타이틀 버튼
    [SerializeField] private CanvasGroup characterSelectGroup; // 캐릭터 선택 버튼
    [SerializeField] private CanvasGroup titleGroup; // 타이틀 추가

    [Header("Background Settings")]
    [SerializeField] private SpriteRenderer backgroundSR; // 'back' 오브젝트 연결
    [SerializeField] private Sprite titleSprite; // 타이틀용 일러스트
    [SerializeField] private Sprite characterSelectSprite; // 캐릭터 선택용 일러스트

    [Header("Character Detail UI")]
    [SerializeField] private CanvasGroup detailInfoGroup; // 캐릭터 이름/설명이 담긴 부모 오브젝트의 CanvasGroup
    [SerializeField] private TMP_Text nameText; // 캐릭터 이름 텍스트
    [SerializeField] private TMP_Text descText; // 캐릭터 설명 텍스트

    [Header("Character Data")]
    [SerializeField] private CharacterInfo[] characters; // 캐릭터 데이터 배열

    [Header("Settings")]
    [SerializeField] private float fadeduration = 0.5f; // 전환 속도
    [SerializeField] private float infoFadeDuration = 0.3f; // 캐릭터 정보 페이드 속도
    [SerializeField] private string gameSceneName = "CharacterBuff"; // 이동할 씬 이름

    private Coroutine infoFadeCoroutine;

    // [Start] 버튼 클릭 시 (메인 -> 캐릭터 선택)
    public void OnStartButtonClick()
    {
        StopAllCoroutines(); // 혹시 실행 중인 페이드가 있다면 멈춤
        StartCoroutine(FadeTransition(true));

        // 배경 이미지를 캐릭터 선택용으로 교체
        if(backgroundSR != null && characterSelectGroup != null)
        {
            backgroundSR.sprite = characterSelectSprite;
        }
    }
    
    // [Cancel] 버튼 클릭 시 (캐릭터 선택 -> 메인)
    public void OnCancelButtonClick()
    {
        StopAllCoroutines();
        StartCoroutine(FadeTransition(false));

        // 배경 이미지를 다시 원래 타이틀용으로 교체
        if(backgroundSR != null && titleSprite != null)
        {
            backgroundSR.sprite = titleSprite;
        }
    }

    /// <summary>
    /// 캐릭터 아이콘 버튼을 눌렀을 때 호출 (인스펙터에서 Index 설정 필요)
    /// </summary>
    public void SelectCharacter(int index)
    {
        if (index < 0 || index >= characters.Length) return;

        // 기존에 실행 중인 정보 페이드 코루틴이 있다면 정지
        if(infoFadeCoroutine != null) StopCoroutine(infoFadeCoroutine);

        // 정보 업데이트 및 페이드 인 시작
        infoFadeCoroutine = StartCoroutine(FadeCharacterInfo(characters[index]));
    }

    private IEnumerator FadeCharacterInfo(CharacterInfo info)
    {
        // 기존 정보를 살짝 가리기 위해 알파를 0으로
        detailInfoGroup.alpha = 0;

        // 데이터 교체
        nameText.text = info.charName;
        descText.text = info.description;
        if (backgroundSR != null) backgroundSR.sprite = info.charSprite;

        // 페이드 인 효과
        float timer = 0f;   
        while(timer < infoFadeDuration)
        {
            timer += Time.deltaTime;
            detailInfoGroup.alpha = Mathf.Lerp(0, 1, timer / infoFadeDuration);
            yield return null;
        }
        detailInfoGroup.alpha = 1;
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
