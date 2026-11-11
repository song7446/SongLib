using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SongLib
{
    public static class SheetJsonSaver
    {
        public static void Save(string json, string savePath)
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("JSON 내용이 비어 있습니다.");
                return;
            }

            if (string.IsNullOrWhiteSpace(savePath))
            {
                Debug.LogError("JSON 저장 경로가 비어 있습니다.");
                return;
            }

            string directory = Path.GetDirectoryName(savePath);

            if (!string.IsNullOrEmpty(directory) &&
                !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(savePath, json);

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif

            Debug.Log($"JSON 저장 완료: {savePath}");
        }
    }
}