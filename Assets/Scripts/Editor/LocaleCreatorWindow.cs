using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LocaleCreatorWindow : EditorWindow
{
    private string csvPath = string.Empty;

    [MenuItem("Tools/CSV Loader")]
    public static void ShowWindow()
    {
        GetWindow<LocaleCreatorWindow>("CSV Loader");
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
        var data = CSVLoader.LoadCSV(csvPath);

        /*Locale newLocale = CreateInstance<Locale>();

        var texts = new List<TextKeyValue>();

        foreach (var entry in data)
        {
            TextKeyValue textKeyValue = new TextKeyValue
            {
                key = entry["Key"],
                value = entry[keyName]
            };

            texts.Add(textKeyValue);
        }

        newLocale.SetTexts(texts);
        string savePath =
            EditorUtility.SaveFilePanelInProject("Save Locale of " + keyName, "New Locale", "asset", "Save locale");
        if (string.IsNullOrEmpty(savePath)) return;

        AssetDatabase.CreateAsset(newLocale, savePath);*/
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
    }
}