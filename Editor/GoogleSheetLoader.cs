using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetLoader : EditorWindow
{
    private UnityWebRequest _request;

    private string _sheetUrl;
    private string _savePath = "Assets/Sheets/";
    private string _sheetName;
    
    [MenuItem("Tools/Google Sheet/Download CSV")]
    public static void Open()
    {
        GetWindow<GoogleSheetLoader>("Google Sheet Loader");
    }

    private void OnGUI()
    {
        GUILayout.Label("Google Sheet → CSV Downloader", EditorStyles.boldLabel);

        GUILayout.Label("CSV URL");
        _sheetUrl = EditorGUILayout.TextField(_sheetUrl);

        GUILayout.Label("Sheet Name");
        _sheetName = EditorGUILayout.TextField(_sheetName);

        GUILayout.Space(10);

        using (new EditorGUI.DisabledScope(string.IsNullOrEmpty(_sheetUrl)))
        {
            if (_request == null)
            {
                if (GUILayout.Button("Download & Save CSV"))
                {
                    StartRequest(_sheetUrl);
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
                _savePath
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
        
        string savePathWithSheetName = savePath+ _sheetName + ".csv";

        var dir = Path.GetDirectoryName(savePathWithSheetName);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        // BOM 제거
        csv = csv.Replace("\uFEFF", "");

        File.WriteAllText(savePathWithSheetName, csv);
        AssetDatabase.Refresh();

        Debug.Log($"✅ CSV saved: {savePathWithSheetName}");
    }
}
