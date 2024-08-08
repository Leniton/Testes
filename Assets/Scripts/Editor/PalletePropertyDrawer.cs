using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using PopupWindow = UnityEngine.UIElements.PopupWindow;

[CustomEditor(typeof(ColorPalette))]
public class PalletePropertyDrawer : Editor
{
    private ColorPalette palette;
    private VisualElement root;
    private Foldout colorsRoot;
    private Foldout referencesRoot;
    private PopupWindow Popup;

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
        root = new();
        root.name = "root";
        
        Button read = new();
        read.text = "ReadProject";
        read.clicked += ReadProjectElements;
        root.Add(read);

        colorsRoot = new Foldout();
        colorsRoot.text = "Colors";
        colorsRoot.style.top = 5;
        root.Add(colorsRoot);
        UpdatePalette();

        referencesRoot = new Foldout();
        referencesRoot.text = "Component References";
        referencesRoot.style.top = 5;
        root.Add(referencesRoot);
        UpdateProjectElements();

        Button apply = new();
        apply.style.top = 5;
        apply.text = "ApplyPallete";
        apply.clicked += ApplyPaletteChanges;
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
        UpdateProjectElements();
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
            b.clicked += () => RemoveColor(id);
            color.Add(b);
            
            ColorField field = new ColorField($"{i}");
            field.value = palette.Colors[id];
            field.RegisterCallback<ChangeEvent<Color>>((evt)=>
            {
                palette.Colors[id] = evt.newValue;
                UpdateProjectElements();
            });
            color.Add(field);
            
            colorsRoot.Add(color);
        }


        Button addButton = new();
        addButton.text = "AddColor";
        addButton.clicked += () =>
        {
            palette.Colors.Add(Color.white);
            UpdatePalette();
        };
        colorsRoot.Add(addButton);
    }

    private void UpdateProjectElements()
    {
        referencesRoot.Clear();
        for (var i = 0; i < palette.Graphics.Count; i++)
        {
            AddPrefabReference(palette.Graphics, palette.graphicsLookUp, i);
        }

        for (var i = 0; i < palette.Renderers.Count; i++)
        {
            AddPrefabReference(palette.Renderers, palette.renderersLookUp, i);
        }
    }

    private void RemoveColor(int id)
    {
        palette.Colors.RemoveAt(id);

        for (int i = 0; i < palette.graphicsLookUp.Count; i++)
        {
            if (palette.graphicsLookUp[i] >= id) palette.graphicsLookUp[i]--;
        }
        for (int i = 0; i < palette.renderersLookUp.Count; i++)
        {
            if (palette.renderersLookUp[i] >= id) palette.renderersLookUp[i]--;
        }

        UpdatePalette();
        UpdateProjectElements();
    }

    private void AddPrefabReference<T>(List<T> reference, List<int> lookUp, int id) where T : Object
    {
        float spacing = 8;
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

        Button color = new Button();
        color.style.left = spacing;
        color.style.width = 20;
        color.style.backgroundColor = palette.Colors[lookUp[id]];
        int childID = referencesRoot.childCount;
        color.clicked += () => ColorsPopup(childID);
        referenceInfo.Add(color);

        ObjectField field = new ObjectField();
        //field.style.flexBasis = 200;
        field.style.left = spacing;
        field.value = reference[id];
        field.objectType = typeof(T);
        referenceInfo.Add(field);

        referencesRoot.Add(referenceInfo);
    }

    private void ColorsPopup(int id)
    {
        if(Popup != null) ClosePopup();

        List<int> lookup = palette.graphicsLookUp;
        if(id >= lookup.Count)
        {
            id -= lookup.Count;
            lookup = palette.renderersLookUp;
        }

        Popup = new PopupWindow();
        Popup.RegisterCallback<FocusOutEvent>((evt) =>
        {
            bool isChild = false;
            VisualElement element = Popup.ElementAt(0);
            int childs = Popup.ElementAt(0).childCount;
            for (int i = 0; i < childs; i++)
            {
                if (element.ElementAt(i) == evt.relatedTarget)
                {
                    isChild = true;
                    break;
                }
            }

            if (!isChild) ClosePopup();
        });

        VisualElement button = referencesRoot.ElementAt(id).ElementAt(1);
        Vector3 position = button.worldTransform.GetPosition() + (Vector3)(button.worldBound.size * .5f);
        position -= root.worldTransform.GetPosition();
        Popup.transform.position = position;

        Popup.text = "Colors";
        float darkness = .2f;
        Popup.style.backgroundColor = new Color(darkness, darkness, darkness, 1);
        Popup.style.position = Position.Absolute;
        Popup.style.flexBasis = root.worldBound.width - position.x;
        Popup.style.width = root.worldBound.width - position.x;

        VisualElement window = new VisualElement();
        window.style.flexDirection = FlexDirection.Row;
        Popup.Add(window);

        for (int i = 0; i < palette.Colors.Count; i++)
        {
            int colorID = i;
            Button color = new Button();
            color.name = "color pick";
            color.style.left = 8;
            color.style.width = 20;
            color.style.height = 20;
            color.style.backgroundColor = palette.Colors[colorID];
            color.clicked += () =>
            {
                lookup[id] = colorID;
                UpdateProjectElements();
                ClosePopup();
            };
            window.Add(color);
        }

        root.Add(Popup);
        FocusElement(Popup, true);
    }

    private void ClosePopup()
    {
        root.Remove(Popup);
        Popup = null;
    }

    private void ApplyPaletteChanges()
    {
        for (int i = 0; i < palette.graphicsLookUp.Count; i++)
        {
            ApplyElementColor(palette.Graphics[i]);
        }
        for (int i = 0; i < palette.renderersLookUp.Count; i++)
        {
            ApplyElementColor(palette.Renderers[i]);
        }
        AssetDatabase.Refresh();
    }
    private void ApplyElementColor(Graphic element)
    {
        string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(element);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        element.color = palette.Colors[palette.graphicsLookUp[palette.Graphics.IndexOf(element)]];

        PrefabUtility.SavePrefabAsset(prefab);
        AssetDatabase.Refresh();
    }
    private void ApplyElementColor(SpriteRenderer element)
    {
        string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(element);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        element.color = palette.Colors[palette.renderersLookUp[palette.Renderers.IndexOf(element)]];

        PrefabUtility.SavePrefabAsset(prefab);
        AssetDatabase.Refresh();
    }

    private static void FocusElement(VisualElement element, bool forceFocusable = false)
    {
        if (!element.focusable)
        {
            if (!forceFocusable) return;
            element.focusable = true;
        }

        bool isRoot = element.parent == null;
        VisualElement root = element;

        while (!isRoot)
        {
            root = root.parent;
            isRoot = root.parent == null;
        }

        Focusable lastFocused = root.panel.focusController.focusedElement;
        FocusOutEvent focusOutEvent = FocusEventBase<FocusOutEvent>.GetPooled(lastFocused, lastFocused, FocusChangeDirection.none, root.panel.focusController);
        FocusEvent focusEvent = FocusEventBase<FocusEvent>.GetPooled(element, lastFocused, FocusChangeDirection.none, root.panel.focusController);

        root.SendEvent(focusOutEvent);
        root.SendEvent(focusEvent);
    }
}
