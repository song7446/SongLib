using System.IO;
using UnityEditor;
using UnityEngine;

namespace SongLib
{
    public static class SheetJsonSaver
    {
        public static void Save(string json, string savePath)
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("❌ JSON is empty");
                return;
            }

            var dir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(savePath, json);
            AssetDatabase.Refresh();

            Debug.Log($"✅ JSON saved: {savePath}");
        }
    }
}
