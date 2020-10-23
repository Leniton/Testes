using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System;
using System.Data;
using UnityEditor.EditorTools;

public class Input_Window : EditorWindow
{
    public int Nbuttons = 3;
    List<string> Names = new List<string>();

    List<SearchField> searchFields = new List<SearchField>();
    List<string> pqs = new List<string>();
    List<string> txt = new List<string>();

    KeyCode kc = KeyCode.A;

    [MenuItem("Window/Custom input")]
    public static void Show_Window()
    {
        GetWindow<Input_Window>();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add"))
        {
            Nbuttons++;
        }

        for (int i = 0; i < Nbuttons; i++)
        {
            EditorGUILayout.BeginHorizontal();

            if (Names.Count != Nbuttons)
            {
                FillLists();
            }

            Names[i] = EditorGUILayout.TextField(Names[i]);

            txt[i] = searchFields[i].OnToolbarGUI(pqs[i]);
            if(txt[i] != pqs[i])
            {
                pqs[i] = txt[i];
            }

            if (searchFields[i].HasFocus())
            {

            }

            if (GUILayout.Button("remove"))
            {
                Names.RemoveAt(i);

                searchFields.RemoveAt(i);
                txt.RemoveAt(i);
                pqs.RemoveAt(i);

                Nbuttons--;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        /*if (GUILayout.Button("criar"))
        {
            Debug.Log("criar");
        }*/
    }

    void FillLists()
    {
        for (int i = 0; i < Nbuttons; i++)
        {
            if(i >= Names.Count)
            {
                Names.Add(i.ToString());
                
            }
            if (i >= pqs.Count)
            {
                searchFields.Add(new SearchField());
                txt.Add("");
                pqs.Add("botão");
            }

        }
        //Custom_Input ci;
    }

    void Sugestions()
    {
        //EditorGUILayout.EnumFlagsField(kc);
        GUI.Label(new Rect(0, 0, 20, 80), "asfgtr");
    }
}
