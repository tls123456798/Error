using UnityEngine;

public class GameExit : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();

        // 유니티 에디터 상에서 플레이 모드를 강제 종료합니다.(테스트용)
        // #if는 유니티 에디터에서만 실행되도록 하는 '전처리기' 입니다.   

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Debug.Log("게임종료");
    }
   
}
