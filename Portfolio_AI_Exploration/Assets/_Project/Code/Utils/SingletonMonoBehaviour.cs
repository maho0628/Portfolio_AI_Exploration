using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>();

                if (_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                    Debug.Log($"[Singleton] {typeof(T).Name} auto-created");
                }
            }
            return _instance;
        }
    }

    [SerializeField] private bool dontDestroyOnLoad = true;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;

            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            OnInitialized(); // ← ★ここが重要
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// 初期化フック（派生クラスで必ず使う）
    /// </summary>
    protected virtual void OnInitialized() { }
}