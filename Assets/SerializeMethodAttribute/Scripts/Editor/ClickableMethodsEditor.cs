using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ClickableMethodsEditor : Editor
{
    private Dictionary<string, object> methodParameters = new();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //Debug.Log(methods.Length);
        if (target.GetType().HasAttribute<SerializeMethods>(true))
        {
            MethodInfo[] methods = target.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance |
                                                               BindingFlags.Public | BindingFlags.NonPublic);
            foreach (MethodInfo method in methods)
            {
                ShowMethod(method);
            }
        }
    }

    private void ShowMethod(MethodInfo method)
    {
        ParameterInfo[] parameters = method.GetParameters();
        GUILayout.BeginHorizontal();
        string methodKey = method.ToString();
        if (!methodParameters.ContainsKey(methodKey)) methodParameters.Add(methodKey,false);
        bool button = GUILayout.Button(method.Name);
        GUILayout.Space(10);
        if(parameters.Length > 0) methodParameters[methodKey] = EditorGUILayout.Foldout((bool)methodParameters[methodKey],$"parameters",true);
        GUILayout.EndHorizontal();
        if (parameters.Length <= 0 || (bool)methodParameters[methodKey])
        {
            object[] methodParams = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                string key = $"{method.ToString()} - {parameters[i].Name}";
                if (!methodParameters.ContainsKey(key)) methodParameters.Add(key, parameters[i].DefaultValue);
                if (methodParameters[key] == null || methodParameters[key].GetType() != parameters[i].ParameterType)
                    methodParameters[key] = parameters[i].ParameterType.Default();
                
                methodParameters[key] = CreateObjectField(methodParameters[key],parameters[i]);
                methodParams[i] = methodParameters[key];
            }
            if (button)
            {
                object returnValue = method.Invoke(target, methodParams);
                if (method.ReturnType != typeof(void))
                {
                    Debug.Log($"the method returns {returnValue}");
                }
            }
        }
    }

    private object CreateObjectField(object currentValue, ParameterInfo parameter)
    {
        object returnObject = (currentValue == null || currentValue.GetType() != parameter.ParameterType)
            ? parameter.ParameterType.Default()
            : currentValue;
        if (parameter.ParameterType == typeof(string) || parameter.ParameterType == typeof(char))
        {
            returnObject = returnObject == null ? string.Empty : returnObject;
            returnObject = EditorGUILayout.TextField(parameter.Name,returnObject.ToString());
        }
        else if (parameter.ParameterType == typeof(int))
        {
            returnObject = EditorGUILayout.IntField(parameter.Name,(int)returnObject);
        }
        else if (parameter.ParameterType == typeof(float))
        {
            returnObject = EditorGUILayout.FloatField(parameter.Name,(float)returnObject);
        }
        else if (parameter.ParameterType == typeof(bool))
        {
            returnObject = EditorGUILayout.Toggle(parameter.Name,(bool)returnObject);
        }
        else if (parameter.ParameterType == typeof(UnityEngine.Color))
        {
            Color value = (returnObject as Color?).HasValue ? (Color)(returnObject as Color?) : default;
            returnObject = EditorGUILayout.ColorField(parameter.Name,value);
        }
        else if (parameter.ParameterType == typeof(UnityEngine.Vector2))
        {
            Vector2 value = (returnObject as Vector2?).HasValue ? (Vector2)(returnObject as Vector2?) : Vector2.zero;
            returnObject = EditorGUILayout.Vector2Field(parameter.Name,value);
        }
        else if (parameter.ParameterType == typeof(UnityEngine.Vector3))
        {
            Vector3 value = (returnObject as Vector3?).HasValue ? (Vector3)(returnObject as Vector3?) : Vector3.zero;
            returnObject = EditorGUILayout.Vector3Field(parameter.Name,value);
        }
        else if (parameter.ParameterType == typeof(UnityEngine.Vector4))
        {
            Vector4 value = (returnObject as Vector4?).HasValue ? (Vector4)(returnObject as Vector4?) : Vector4.zero;
            returnObject = EditorGUILayout.Vector4Field(parameter.Name,value);
        }
        else if (parameter.ParameterType == typeof(UnityEngine.Rect))
        {
            Rect value = (returnObject as Rect?).HasValue ? (Rect)(returnObject as Rect?) : Rect.zero;
            returnObject = EditorGUILayout.RectField(parameter.Name,value);
        }
        else if (parameter.ParameterType == typeof(UnityEngine.Bounds))
        {
            Bounds value = (returnObject as Bounds?).HasValue ? (Bounds)(returnObject as Bounds?) : default;
            returnObject = EditorGUILayout.BoundsField(parameter.Name,value);
        }
        else if (parameter.ParameterType.IsReferenceType())
        {
            returnObject = EditorGUILayout.ObjectField(parameter.Name,(Object)returnObject, parameter.ParameterType, true);
        }
        else
        {
            GUILayout.Label($"{parameter.ParameterType} is an unsupported type");
        }
        return returnObject;
    }
}
