using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveCsvWindow : EditorWindow
{
    private string csvPath = string.Empty;

    [MenuItem("Tools/CSV Loader")]
    public static void ShowWindow()
    {
        GetWindow<SaveCsvWindow>("CSV Loader");
    }

    private void OnGUI()
    {
        GUILayout.Label("Load CSV", EditorStyles.boldLabel);

        if (GUILayout.Button("Load CSV"))
        {
            csvPath = EditorUtility.OpenFilePanel("Load CSV File", "", "csv");
        }

        if (!string.IsNullOrEmpty(csvPath) && GUILayout.Button("Generate Scriptable Object"))
        {
            GenerateLocale();
        }
    }

    private void GenerateLocale()
    {
        var data = CSVLoader.LoadCSV(new StreamReader(csvPath));

        CSV_String_Values newData = CreateInstance<CSV_String_Values>();
        newData.Values = data;
        
        string savePath =
            EditorUtility.SaveFilePanelInProject("Save Csv of ", "New Csv", "asset", "Save locale");
        if (string.IsNullOrEmpty(savePath)) return;

        AssetDatabase.CreateAsset(newData, savePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}