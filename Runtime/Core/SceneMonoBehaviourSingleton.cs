using UnityEngine;

public abstract class SceneMonoBehaviourSingleton<T> : MonoBehaviour
    where T : SceneMonoBehaviourSingleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<T>(
                    FindObjectsInactive.Include
                );
            }

            return instance;
        }
    }

    // 종료 처리에서 자동 탐색 없이 살아 있는 인스턴스만 확인.
    public static bool TryGetInstance(out T result)
    {
        result = instance;
        return result != null;
    }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}