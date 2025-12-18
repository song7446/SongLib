using UnityEngine.SceneManagement;
using System;
using SongLib.Core.Singleton;

namespace SongLib
{
    public class SceneLoader : MonoBehaviourSingleton<SceneLoader>
    {
        public event Action OnSceneLoaded;

        public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
            SceneManager.LoadScene(sceneName, mode);
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
            OnSceneLoaded?.Invoke();
        }
    }
}
