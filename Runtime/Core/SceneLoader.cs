using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SongLib.Core
{
    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader _instance;
        public static SceneLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("SceneLoader");
                    _instance = go.AddComponent<SceneLoader>();
                    Object.DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        public void LoadScene(string sceneName)
        {
            Instance.StartCoroutine(LoadSceneRoutine(sceneName));
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            var async = SceneManager.LoadSceneAsync(sceneName);
            async.allowSceneActivation = false;

            while (!async.isDone)
            {
                if (async.progress >= 0.9f)
                    async.allowSceneActivation = true;
                yield return null;
            }
        }
    }
}
