using UnityEngine;

/// <summary>
/// 유니티 MonoBehaviour 기반의 싱글톤 기본 클래스입니다.
/// 이 클래스를 상속받으면 '클래스이름.Instance'를 통해 어디서든 해당 시스템에 접근할 수 있습니다.
/// </summary>
/// <typeparam name="T">싱글톤으로 만들고자 하는 컴포넌트 타입</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 외부에서는 읽기만 가능하고, 내부에서만 설정할 수 있는 정적 인스턴스 변수
    public static T Instance { get; private set; }

    /// <summary>
    /// 객체가 생성될 때 호출됩니다. (유니티 생명주기)
    /// 중복된 인스턴스가 생성되는 것을 방지하는 핵심 로직이 담겨 있습니다.
    /// </summary>
    protected virtual void Awake()
    {
        // 이미 인스턴스가 존재한다면 (중복 생성 시도)
        if (Instance != null)
        {
            // 나중에 생성된 객체를 즉시 파괴하여 단일성을 유지합니다.
            Destroy(gameObject);
            return;
        }
        // 인스턴스가 없다면 현재 자기 자신(this)을 정적 변수에 할당합니다.
        Instance = this as T;
    }

    /// <summary>
    /// 게임 애플리케이션이 종료될 때 호출 됩니다.
    /// 메모리 정리 및 참조 해제를 수행합니다.
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// 씬이 변경되어도 파괴되지 않는 싱글톤 클래스입니다.
/// 게임 매니저, 오디오 매니저 등 겡미 전체 세션 동안 유지되어야 하는 시스템에 사용합니다.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        // 부모 클래스의 Awake를 호출하여 인스턴스 할당 및 중복 체크를 먼저 수행합니다.
        base.Awake();

        // 인스턴스가 정상적으로 할당된 경우, 씬이 바뀌어도 오브젝트가 파괴되지 않도록 설정합니다.
        DontDestroyOnLoad(gameObject);
    }
}
