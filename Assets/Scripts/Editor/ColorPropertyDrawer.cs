using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

[CustomEditor(typeof(ColorPalette))]
public class ColorPropertyDrawer : Editor
{
    private ColorPalette palette;
    private VisualElement colorsRoot;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new();
        
        palette = target as ColorPalette;
        
        Button read = new();
        read.text = "ReadScene";
        read.clicked += ReadProjectElements;
        root.Add(read);

        Button addButton = new();
        addButton.text = "AddColor";
        addButton.clicked += () =>
        {
            palette.Colors.Add(Color.white);
            UpdatePalette();
        };
        
        root.Add(addButton);

        colorsRoot = new VisualElement();
        colorsRoot.style.top = 5;
        root.Add(colorsRoot);
        UpdatePalette();
        
        Button apply = new();
        apply.style.top = 5;
        apply.text = "ApplyPallete";
        apply.clicked += UpdateProjectElements;
        root.Add(apply);

        return root;
    }

    private void UpdatePalette()
    {
        colorsRoot.Clear();
        for (var i = 0; i < palette.Colors.Count; i++)
        {
            int id = i;
            VisualElement color = new();
            color.style.flexDirection = FlexDirection.Row;

            Button b = new Button();
            b.style.width = 20;
            b.text = "X";
            b.clicked += () =>
            {
                palette.Colors.RemoveAt(id);
                UpdatePalette();
            };
            color.Add(b);
            
            ColorField field = new ColorField($"{i}");
            field.value = palette.Colors[id];
            field.RegisterCallback<ChangeEvent<Color>>((evt)=>
            {
                palette.Colors[id] = evt.newValue;
            });
            color.Add(field);
            
            colorsRoot.Add(color);
        }
    }

    private void ReadProjectElements()
    {
        string[] guids = AssetDatabase.FindAssets( "t:Prefab" , new string[] { "Assets" });
        colorsRoot.Clear();
        palette.Graphics.Clear();
        //GetPrefabAssetPathOfNearestInstanceRoot	Retrieves the asset path of the nearest Prefab instance root the specified object is part of.
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath( guid );
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>( path );

            Graphic[] graphics = go.GetComponentsInChildren<Graphic>();
            for (int i = 0; i < graphics.Length; i++)
            {
                if (!PrefabUtility.IsPartOfPrefabInstance(graphics[i]))
                {
                    Debug.Log($"{go} | {graphics[i]} \n {PrefabUtility.IsPartOfPrefabInstance(graphics[i])}");
                    palette.Graphics.Add(graphics[i]);
                }
            }
        }
        
        for (var i = 0; i < palette.Graphics.Count; i++)
        {
            ObjectField field = new ObjectField();
            field.value = palette.Graphics[i];
            colorsRoot.Add(field);
        }
    }

    private void UpdateProjectElements()
    {
        
    }
}
