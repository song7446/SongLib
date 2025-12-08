using UnityEngine;

namespace SongLib.Core.Singleton
{
    /// <summary>
    /// 씬에 자동으로 생성되는 전역 싱글톤 베이스 클래스
    /// </summary>
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindFirstObjectByType<T>();
                if (_instance == null)
                {
                    var obj = new GameObject(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
    }
}