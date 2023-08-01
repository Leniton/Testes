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

    List<KeyCode> keycodes = new List<KeyCode>();
    List<string> teclas = new List<string>();

    public bool checking;
    Input_Data data;
    bool emptySlot = true;

    [MenuItem("Window/Custom input")]
    public static void Show_Window()
    {
        GetWindow<Input_Window>();
    }

    private void OnGUI()
    {
        if (!emptySlot && data == null)
        {
            emptySlot = true;
        }

        data = (Input_Data)EditorGUILayout.ObjectField("Scriptable object",data, typeof(Input_Data),true);

        if (data != null && emptySlot)
        {
            Nbuttons = data.inputs.Count;
            emptySlot = false;
        }

        if (GUILayout.Button("Add"))
        {
            Nbuttons++;
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Atualizar lista de teclas"))
        {
            CriarTeclas();
        }

        GUILayout.Space(5);

        for (int i = 0; i < Nbuttons; i++)
        {
            EditorGUILayout.BeginHorizontal();

            if (Names.Count != Nbuttons)
            {
                if (data != null)
                {
                    FillLists(data);
                }
                else
                {
                    FillLists();
                }
            }

            Names[i] = EditorGUILayout.TextField("Nome da ação:",Names[i]);

            if (GUILayout.Button(teclas[i],GUILayout.Width(150)))
            {
                //Debug.Log("clicou");
                if (!teclas.Contains("aguardando input"))
                {
                    //Debug.Log("checando..");
                    teclas[i] = "aguardando input";
                    checking = true;
                }
                else if(teclas[i] == "aguardando input")
                {
                    teclas[i] = "Registrar tecla";
                    checking = false;
                }
            }

            if (GUILayout.Button("remover",GUILayout.Width(70)))
            {
                Names.RemoveAt(i);

                keycodes.RemoveAt(i);
                teclas.RemoveAt(i);

                Nbuttons--;
            }

            EditorGUILayout.EndHorizontal();
        }

        

        if (checking)
        {
            Event e = Event.current;
            if (e != null && (e.type == EventType.MouseDown || e.type == EventType.KeyDown))
            {
                //checkButton();
                //Debug.Log("opa");
                checking = false;
                keyPressed(e);
            }
        }
    }

    void keyPressed(Event e)
    {
        int index = teclas.IndexOf("aguardando input");

        if(e.type == EventType.MouseDown)
        {
            //323 valor do botão 0 do mouse
            keycodes[index] = (KeyCode)323 + e.button;
            teclas[index] = "Mouse" + e.button;
        }
        else
        {
            keycodes[index] = e.keyCode;
            teclas[index] = e.keyCode.ToString();
        }

        Debug.Log("A ação \'" + Names[index] + "\' agora é a tecla \'" + keycodes[index].ToString() + "\'");
    }

    void FillLists()
    {
        for (int i = 0; i < Nbuttons; i++)
        {
            if(i >= Names.Count)
            {
                Names.Add(i.ToString());
                
            }
            if (i >= teclas.Count)
            {
                keycodes.Add(KeyCode.None);
                teclas.Add("Registrar tecla");
            }

        }
        //Custom_Input ci;
    }

    void FillLists(Input_Data d)
    {
        int n = 0;

        Names.Clear();

        teclas.Clear();
        keycodes.Clear();

        foreach (KeyValuePair<string,KeyCode> keys in data.inputs)
        {
            Names.Add(keys.Key);

            keycodes.Add(keys.Value);
            if((int)keys.Value > 322)
            {
                teclas.Add("Mouse" + ((int)keys.Value - 323).ToString());
            }
            else
            {
                teclas.Add(keys.Value.ToString());
            }
            n++;
        }

        if(n < Nbuttons)
        {
            for (int i = n; i < Nbuttons; i++)
            {
                Names.Add(i.ToString());

                teclas.Add("Registrar tecla");
                keycodes.Add(KeyCode.None);
            }
        }
    }

    void CriarTeclas()
    {
        Debug.Log("criar");

        data.inputs.Clear();

        for (int i = 0; i < Nbuttons; i++)
        {
            data.inputs.Add(Names[i], keycodes[i]);
            //Debug.Log("Adicionado a ação \'" + Names[i] + "\' que é a tecla \'" + keycodes[i].ToString() + "\'");
        }
    }
}
