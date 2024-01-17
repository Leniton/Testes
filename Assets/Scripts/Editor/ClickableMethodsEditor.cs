using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ClickableMethodsEditor : Editor
{
    private Dictionary<string, object> methodParameters = new();

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new();
        root.Add(new IMGUIContainer(OnInspectorGUI));
        
        MethodInfo[] methods = SerializeMethodHelper.GetMethods(target.GetType());
        foreach (MethodInfo method in methods)
        {
            if (method.HasAttribute<SerializeMethod>())
            {
                SerializeMethodHelper.ShowMethod(target.GameObject(), method, root);
            }
        }

        return root;
    }
}
