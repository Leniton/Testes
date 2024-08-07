using System.Collections.Generic;
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
        palette.graphicsLookUp= new();
        palette.Renderers.Clear();
        palette.renderersLookUp.Clear();
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
        palette.graphicsLookUp.Clear();
        palette.Renderers.Clear();
        palette.renderersLookUp.Clear();

        string[] prefabs = AssetDatabase.FindAssets( "t:Prefab" , palette.searchFolders);
        //string[] scenes =  AssetDatabase.FindAssets("t:Scene", new string[] { "Assets" });
        //GetPrefabAssetPathOfNearestInstanceRoot	Retrieves the asset path of the nearest Prefab instance root the specified object is part of.
        foreach (var prefab in prefabs)
        {
            var path = AssetDatabase.GUIDToAssetPath( prefab );
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
                    palette.graphicsLookUp.Add(palette.Colors.IndexOf(color));
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
                    palette.renderersLookUp.Add(palette.Colors.IndexOf(color));
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
            //color.style.flexDirection = FlexDirection.Row;

            /*Button b = new Button();
            b.style.width = 20;
            b.text = "X";
            b.clicked += () =>
            {
                palette.Colors.RemoveAt(id);
                UpdatePalette();
            };
            color.Add(b);*/
            
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
        for (var i = 0; i < palette.Graphics.Count; i++)
        {
            referencesRoot.Add(PrefabReference(palette.Graphics, palette.graphicsLookUp, i));
        }

        for (var i = 0; i < palette.Renderers.Count; i++)
        {
            referencesRoot.Add(PrefabReference(palette.Renderers, palette.renderersLookUp, i));
        }
    }

    private VisualElement PrefabReference<T>(List<T> reference, List<int> lookUp, int id) where T : Object
    {
        float spacing = 15;
        VisualElement referenceInfo = new VisualElement();
        referenceInfo.style.flexDirection = FlexDirection.Row;
        referenceInfo.style.height = 25;
        referenceInfo.style.borderTopWidth = 2;
        referenceInfo.style.borderBottomWidth = 2;

        Label index = new Label(lookUp[id].ToString());
        index.style.unityTextAlign = TextAnchor.MiddleCenter;
        index.style.flexBasis = 20;
        float darkness = .1f;
        index.style.backgroundColor = new Color(darkness, darkness, darkness, 1);
        referenceInfo.Add(index);

        VisualElement color = new Button();
        color.style.left = spacing;
        color.style.width = 20;
        color.style.backgroundColor = palette.Colors[lookUp[id]];
        referenceInfo.Add(color);

        ObjectField field = new ObjectField();
        //field.style.flexBasis = 200;
        field.style.left = spacing;
        field.value = reference[id];
        field.objectType = typeof(T);
        referenceInfo.Add(field);

        return referenceInfo;
    }

    private void UpdateProjectElements()
    {
        for (int i = 0; i < palette.graphicsLookUp.Count; i++)
        {
            UpdateElementColor(palette.Graphics[i]);
        }
        for (int i = 0; i < palette.renderersLookUp.Count; i++)
        {
            UpdateElementColor(palette.Renderers[i]);
        }
        AssetDatabase.Refresh();
    }
    private void UpdateElementColor(Graphic element)
    {
        string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(element);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        element.color = palette.Colors[palette.graphicsLookUp[palette.Graphics.IndexOf(element)]];

        PrefabUtility.SavePrefabAsset(prefab);
        AssetDatabase.Refresh();
    }
    private void UpdateElementColor(SpriteRenderer element)
    {
        string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(element);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        element.color = palette.Colors[palette.renderersLookUp[palette.Renderers.IndexOf(element)]];

        PrefabUtility.SavePrefabAsset(prefab);
        AssetDatabase.Refresh();
    }
}
