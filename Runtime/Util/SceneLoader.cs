using UnityEngine.SceneManagement;
using System;
using SongLib.Core.Singleton;

namespace SongLib
{
    public class SceneLoader : MonoBehaviourSingleton<SceneLoader>
    {
        public event Action OnSceneLoaded;   // Awake/OnEnable 끝
        public event Action OnSceneReady;    // Start 끝 (🔥 핵심)

        public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
            SceneManager.LoadScene(sceneName, mode);
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;

            OnSceneLoaded?.Invoke();

            // 🔥 Start() 전부 끝난 뒤
            StartCoroutine(NotifySceneReady());
        }

        private System.Collections.IEnumerator NotifySceneReady()
        {
            yield return null; // 한 프레임 대기
            OnSceneReady?.Invoke();
        }
    }
}
