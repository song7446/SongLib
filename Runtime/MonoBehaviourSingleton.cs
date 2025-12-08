using UnityEngine;

namespace SongLib.Core
{
    /// <summary>
    /// Generic MonoBehaviour Singleton.
    /// Automatically creates an instance if none exists.
    /// </summary>
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
    {
        private static T _instance;
        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = FindObjectOfType<T>();
                        if (_instance == null)
                        {
                            var obj = new GameObject(typeof(T).Name);
                            _instance = obj.AddComponent<T>();
                            DontDestroyOnLoad(obj);
                        }
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}