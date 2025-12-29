using UnityEngine;
using UnityEngine.SceneManagement; // 씬 이동을 위해 추가

public class BonusUIHandler : MonoBehaviour
{
    // 인스펙터에서 우리가 만든 ScriptableObject 아이템들을 드래그 해서 넣어줍니다.
    [SerializeField] private BonusItem myItemData;
    [SerializeField] private string nextSceneName = "Map"; // 이동할 씬 이름

    public void OnClickSelect()
    {
        if(myItemData != null)
        {
            // 금고(DataManager)에 내가 선택한 아이템 정보를 저장
            GameDateManager.Instance.selectedBonus = myItemData;

            // 실제 게임 씬으로 이동 (인스펙터에서 설정한 이름으로)
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
