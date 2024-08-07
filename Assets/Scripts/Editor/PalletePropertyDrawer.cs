using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

[CustomEditor(typeof(ColorPalette))]
public class PalletePropertyDrawer : Editor
{
    private ColorPalette palette;
    private Foldout colorsRoot;
    private Foldout referencesRoot;

    public override VisualElement CreateInspectorGUI()
    {
        palette = target as ColorPalette;
        EditorUtility.SetDirty(palette);
        /*
        palette.Colors.Clear();
        palette.Graphics.Clear();
        palette._graphicsLookUp= new();
        palette.Renderers.Clear();
        palette._renderersLookUp.Clear();
        */
        VisualElement root = new();
        
        
        Button read = new();
        read.text = "ReadProject";
        read.clicked += ReadProjectElements;
        root.Add(read);

        /*Button addButton = new();
        addButton.text = "AddColor";
        addButton.clicked += () =>
        {
            palette.Colors.Add(Color.white);
            UpdatePalette();
        };
        root.Add(addButton);*/

        colorsRoot = new Foldout();
        colorsRoot.text = "Colors";
        colorsRoot.style.top = 5;
        root.Add(colorsRoot);
        UpdatePalette();

        referencesRoot = new Foldout();
        referencesRoot.text = "Component References";
        referencesRoot.style.top = 5;
        root.Add(referencesRoot);
        LoadReferences();

        Button apply = new();
        apply.style.top = 5;
        apply.text = "ApplyPallete";
        apply.clicked += UpdateProjectElements;
        root.Add(apply);

        return root;
    }

    private void ReadProjectElements()
    {
        palette.Colors.Clear();
        palette.Graphics.Clear();
        palette._graphicsLookUp.Clear();
        palette.Renderers.Clear();
        palette._renderersLookUp.Clear();

        string[] guids = AssetDatabase.FindAssets( "t:Prefab" , new string[] { "Assets" });
        //Debug.Log(AssetDatabase.FindAssets("t:Scene", new string[] { "Assets" }).Length);
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
                    //Debug.Log($"{go} | {graphics[i]} \n {PrefabUtility.IsPartOfPrefabInstance(graphics[i])}");
                    palette.Graphics.Add(graphics[i]);
                    Color color = graphics[i].color;
                    //add color if doesn't exist
                    if (!palette.Colors.Contains(color)) palette.Colors.Add(color);
                    //set a lookup inder to link object to color
                    palette._graphicsLookUp.Add(palette.Colors.IndexOf(color));
                }
            }

            SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                if (!PrefabUtility.IsPartOfPrefabInstance(renderers[i]))
                {
                    //Debug.Log($"{go} | {graphics[i]} \n {PrefabUtility.IsPartOfPrefabInstance(graphics[i])}");
                    palette.Renderers.Add(renderers[i]);
                    Color color = renderers[i].color;
                    //add color if doesn't exist
                    if (!palette.Colors.Contains(color)) palette.Colors.Add(color);
                    //set a lookup inder to link object to color
                    palette._renderersLookUp.Add(palette.Colors.IndexOf(color));
                }
            }
        }

        UpdatePalette();
        LoadReferences();
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
                LoadReferences();
            });
            color.Add(field);
            
            colorsRoot.Add(color);
        }
    }

    private void LoadReferences()
    {
        referencesRoot.Clear();
        float spacing = 15;
        for (var i = 0; i < palette.Graphics.Count; i++)
        {
            VisualElement referenceInfo = new VisualElement();
            referenceInfo.style.flexDirection = FlexDirection.Row;
            referenceInfo.style.height = 25;
            referenceInfo.style.borderTopWidth = 2;
            referenceInfo.style.borderBottomWidth = 2;

            ObjectField field = new ObjectField();
            field.style.flexBasis = 200;
            field.style.right = spacing;
            field.value = palette.Graphics[i];
            referenceInfo.Add(field);
            Label index = new Label(palette._graphicsLookUp[i].ToString());
            index.style.unityTextAlign = TextAnchor.MiddleCenter;
            index.style.flexBasis = 20; 
            float darkness = .1f;
            index.style.backgroundColor = new Color(darkness, darkness, darkness, 1);
            referenceInfo.Add(index);

            VisualElement color = new Box();
            color.style.left = spacing;
            color.style.width = 20;
            color.style.backgroundColor = palette.Colors[palette._graphicsLookUp[i]];
            referenceInfo.Add(color);

            referencesRoot.Add(referenceInfo);
        }

        for (var i = 0; i < palette.Renderers.Count; i++)
        {
            VisualElement referenceInfo = new VisualElement();
            referenceInfo.style.flexDirection = FlexDirection.Row;
            referenceInfo.style.height = 25;
            referenceInfo.style.borderTopWidth = 2;
            referenceInfo.style.borderBottomWidth = 2;

            ObjectField field = new ObjectField();
            field.style.flexBasis = 200;
            field.style.right = spacing;
            field.value = palette.Renderers[i];
            referenceInfo.Add(field);

            Label index = new Label(palette._renderersLookUp[i].ToString());
            index.style.unityTextAlign = TextAnchor.MiddleCenter;
            index.style.flexBasis = 20;
            float darkness = .1f;
            index.style.backgroundColor = new Color(darkness, darkness, darkness, 1);
            referenceInfo.Add(index);

            VisualElement color = new Box();
            color.style.left = spacing;
            color.style.width = 20;
            color.style.backgroundColor = palette.Colors[palette._renderersLookUp[i]];
            referenceInfo.Add(color);

            referencesRoot.Add(referenceInfo);
        }
    }

    private void UpdateProjectElements()
    {
        for (int i = 0; i < palette._graphicsLookUp.Count; i++)
        {
            UpdateElementColor(palette.Graphics[i]);
        }
        for (int i = 0; i < palette._renderersLookUp.Count; i++)
        {
            UpdateElementColor(palette.Renderers[i]);
        }
        AssetDatabase.Refresh();
    }

    private void UpdateElementColor(Graphic element)
    {
        string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(element);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        element.color = palette.Colors[palette._graphicsLookUp[palette.Graphics.IndexOf(element)]];

        PrefabUtility.SavePrefabAsset(prefab);
        AssetDatabase.Refresh();
    }

    private void UpdateElementColor(SpriteRenderer element)
    {
        string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(element);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        element.color = palette.Colors[palette._renderersLookUp[palette.Renderers.IndexOf(element)]];

        PrefabUtility.SavePrefabAsset(prefab);
        AssetDatabase.Refresh();
    }
}
