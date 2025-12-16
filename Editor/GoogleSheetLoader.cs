using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetLoader : EditorWindow
{
    private UnityWebRequest _request;

    private string sheetUrl;
    private string savePath = "Assets/Sheets/dialogue.csv";

    private const string UrlPrefKey = "GoogleSheetLoader_URL";

    [MenuItem("Tools/Google Sheet/Download CSV")]
    public static void Open()
    {
        GetWindow<GoogleSheetLoader>("Google Sheet Loader");
    }

    private void OnEnable()
    {
        sheetUrl = EditorPrefs.GetString(UrlPrefKey, "");
    }

    private void OnGUI()
    {
        GUILayout.Label("Google Sheet → CSV Downloader", EditorStyles.boldLabel);

        GUILayout.Label("CSV URL");
        sheetUrl = EditorGUILayout.TextArea(sheetUrl, GUILayout.Height(50));

        GUILayout.Label("Save Path");
        savePath = EditorGUILayout.TextField(savePath);

        if (GUI.changed)
            EditorPrefs.SetString(UrlPrefKey, sheetUrl);

        GUILayout.Space(10);

        using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(sheetUrl)))
        {
            if (_request == null)
            {
                if (GUILayout.Button("Download & Save CSV"))
                {
                    StartRequest(sheetUrl);
                }
            }
            else
            {
                GUILayout.Label("Downloading...");
            }
        }
    }

    private void StartRequest(string url)
    {
        _request = UnityWebRequest.Get(url);
        _request.SendWebRequest();

        EditorApplication.update += OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {
        if (!_request.isDone)
            return;

        EditorApplication.update -= OnEditorUpdate;

        if (_request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"❌ CSV Load Failed: {_request.error}");
        }
        else
        {
            SaveCsv(
                _request.downloadHandler.text,
                savePath
            );
        }

        _request.Dispose();
        _request = null;
    }
    
    public void SaveCsv(string csv, string savePath)
    {
        if (string.IsNullOrEmpty(csv))
        {
            Debug.LogError("❌ CSV is empty");
            return;
        }

        var dir = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        // BOM 제거
        csv = csv.Replace("\uFEFF", "");

        File.WriteAllText(savePath, csv);
        AssetDatabase.Refresh();

        Debug.Log($"✅ CSV saved: {savePath}");
    }
}
