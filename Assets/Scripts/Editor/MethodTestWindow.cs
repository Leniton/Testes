using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Reflection;
using Unity.VisualScripting;
using Object = UnityEngine.Object;

public class MethodTestWindow : EditorWindow
{
    //const fields
    private const string TargetObjectLabel = "Target Object";
    private const string PickClassLabel = "Pick class:";
    private const string PickClassDropdown = "Pick a class:";
    private const string MethodAreaName = "Method area";
    private const string ReturnValue = "Return value";

    //saved values
    MethodTestData data = new();
    private GameObject target = null;
    private int classChoice = -1;

    private Dictionary<string, object> methodParameters
    {
        get => data.methodParameters;
    }

    private string MethodKey(MethodInfo method) => method.ToString();

    private string ParameterKey(MethodInfo method, ParameterInfo parameter) =>
        $"{MethodKey(method)} - {parameter.Name}";

    [MenuItem("Tools/MethodTesting")]
    public static void ShowWindow()
    {
        MethodTestWindow window = GetWindow<MethodTestWindow>();
        window.titleContent = new GUIContent("MethodTesting");
    }

    public void CreateGUI()
    { 
        //data.DeleteSave();
        //methodParameters.Clear();
        VisualElement root = rootVisualElement;
        int padding = 5;
        root.style.paddingTop = padding;
        root.style.paddingBottom = padding;
        root.style.paddingLeft = padding;
        root.style.paddingRight = padding;

        VisualElement Header = new VisualElement();
        Header.style.flexDirection = FlexDirection.Row;
        //Create Elements
        string label = TargetObjectLabel;
        ObjectField targetObjectField = new ObjectField(label);
        targetObjectField.name = label;
        targetObjectField.style.flexGrow = .2f;
        targetObjectField.objectType = typeof(GameObject);
        targetObjectField.RegisterCallback<ChangeEvent<UnityEngine.Object>>(LoadClassOptions);

        VisualElement pickClassArea = new VisualElement();
        pickClassArea.name = PickClassLabel;
        pickClassArea.style.flexGrow = 1;

        VisualElement methodsArea = new Image();
        methodsArea.name = MethodAreaName;
        methodsArea.style.flexDirection = FlexDirection.Row;
        methodsArea.style.flexWrap = Wrap.Wrap;

        //add elements
        Header.Add(targetObjectField);
        Header.Add(pickClassArea);
        root.Add(Header);
        root.Add(methodsArea);

        targetObjectField.value = target;
    }

    private void LoadClassOptions(ChangeEvent<UnityEngine.Object> evt)
    {
        VisualElement area = rootVisualElement.Q(PickClassLabel);
        area.Clear();
        if (evt.newValue.GetType() != typeof(GameObject) || evt.newValue == null)
        {
            target = null;
            return;
        }

        target = (GameObject)evt.newValue;

        //dropDown For picking class
        DropdownField dropdownField = new DropdownField(PickClassDropdown);
        dropdownField.name = PickClassDropdown;
        List<string> options = new();
        Component[] components = target.GetComponents<Component>();
        for (int i = 1; i < components.Length; i++) //starts at 1 to ignore the transform component
            options.Add(components[i].GetType().Name);
        dropdownField.choices = options;

        dropdownField.RegisterCallback<ChangeEvent<string>>(GetClass);

        area.Add(dropdownField);
        if (classChoice >= 0 && classChoice < dropdownField.choices.Count)
            dropdownField.value = dropdownField.choices[classChoice];
    }

    private void GetClass(ChangeEvent<string> evt)
    {
        DropdownField dropdown = rootVisualElement.Q<DropdownField>(PickClassDropdown);
        if (!dropdown.choices.Contains(evt.newValue)) return;

        classChoice = dropdown.choices.IndexOf(evt.newValue);

        LoadMethods(target.GetComponents<Component>()[classChoice + 1].GetType()); //still ignoring transform component
    }

    private void LoadMethods(Type targetClass)
    {
        MethodInfo[] methods = targetClass.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance |
                                                      BindingFlags.Public | BindingFlags.NonPublic);
        VisualElement methodsArea = rootVisualElement.Q(MethodAreaName);
        methodsArea.Clear();
        foreach (MethodInfo method in methods)
        {
            ShowMethod(method, methodsArea);
        }
        
        data.Save();
    }

    private void ShowMethod(MethodInfo method, VisualElement methodsArea)
    {
        VisualElement area = new VisualElement();
        float darkness = .18f;
        area.style.backgroundColor = new Color(darkness, darkness, darkness, 1);
        area.style.width = 250;
        int margin = 2;
        area.style.marginTop = margin;
        area.style.marginBottom = margin;
        area.style.marginLeft = margin;
        area.style.marginRight = margin;
        area.style.flexDirection = FlexDirection.Column;
        methodsArea.Add(area);

        ParameterInfo[] parameters = method.GetParameters();
        string methodKey = MethodKey(method);
        /*if (!methodParameters.ContainsKey(methodKey)) methodParameters.Add(methodKey, parameters);
        else methodParameters[methodKey] = parameters;*/

        if (parameters.Length > 0)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                string key = ParameterKey(method, parameters[i]);
                if (!methodParameters.ContainsKey(key)) methodParameters.Add(key, null);
                CreateObjectField(method, parameters[i], area);
            }
        }

        Button invokeMethod = new Button();
        string returnType = method.ReturnType != typeof(void) ? $"({method.ReturnType.Name})-" : string.Empty;
        invokeMethod.text = $"{returnType}{method.Name}";
        invokeMethod.clicked += () =>
        {
            object[] methodParams = new object[parameters.Length];
            if (parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    methodParams[i] = methodParameters[ParameterKey(method, parameters[i])];
                }
            }

            object returnValue = method.Invoke(target.GetComponent(method.ReflectedType), methodParams);
            if (returnValue != null)
            {
                Label returnLabel = area.Q<Label>(ReturnValue);
                returnLabel.text = $"returned ({returnValue.GetType()})[{returnValue}]";
                SetValue($"{methodKey} - Return:", returnValue);
            }
        };
        area.Add(invokeMethod);
        if (method.ReturnType != typeof(void))
        { 
            Label returnLabel = new Label();
            returnLabel.name = ReturnValue;
            string key = $"{methodKey} - Return:";
            if(methodParameters.ContainsKey(key)) 
                returnLabel.text = $"last return: ({methodParameters[key].GetType().Name})[{methodParameters[key]}]";
            area.Add(returnLabel);
        }
    }

    private void CreateObjectField(MethodInfo method, ParameterInfo parameter, VisualElement objectParent)
    {
        string key = ParameterKey(method, parameter);
        if (!methodParameters.ContainsKey(key)) methodParameters.Add(key, parameter.ParameterType.Default());
        if (methodParameters[key] == null) methodParameters[key] = parameter.ParameterType.Default();

        string label = parameter.Name;
        object returnObject = methodParameters[key];
        if (parameter.ParameterType == typeof(string) || parameter.ParameterType == typeof(char))
        {
            returnObject = returnObject == null ? string.Empty : returnObject;
            TextField field = new TextField(label);
            field.value = returnObject.ToString();
            //methodParameters[key] = field.value;
            field.RegisterCallback<ChangeEvent<string>>((evt) => SetValue(key,evt.newValue));
            objectParent.Add(field);
        }
        else if (parameter.ParameterType == typeof(int))
        {
            IntegerField field = new IntegerField(label);
            field.value = (int)returnObject;
            field.RegisterCallback<ChangeEvent<int>>((evt) => SetValue(key,evt.newValue));
            objectParent.Add(field);
            //returnObject = EditorGUILayout.IntField(label, (int)returnObject, width);
        }
        else if (parameter.ParameterType == typeof(float))
        { 
            FloatField field = new FloatField(label);
            field.value = (float)(Convert.ToDecimal(returnObject));
            field.RegisterCallback<ChangeEvent<float>>((evt) => SetValue(key,evt.newValue));
            objectParent.Add(field);
            //returnObject = EditorGUILayout.FloatField(label, (float)returnObject, width);
        }
        else if (parameter.ParameterType == typeof(bool))
        {
            Toggle field = new Toggle(label);
            field.value = (bool)returnObject;
            field.RegisterCallback<ChangeEvent<bool>>((evt) => SetValue(key,evt.newValue));
            objectParent.Add(field);
            //returnObject = EditorGUILayout.Toggle(label, (bool)returnObject, width);
        }
        else if (parameter.ParameterType == typeof(Color))
        {
            Color value = (returnObject as Color?).HasValue ? (Color)(returnObject as Color?) : default;
            ColorField field = new ColorField();
            field.value = value;
            field.RegisterCallback<ChangeEvent<Color>>(evt => SetValue(key, evt.newValue));
            objectParent.Add(field);
        }
        else if (parameter.ParameterType == typeof(Vector2))
        {
            Vector2 value = (returnObject as Vector2?).HasValue ? (Vector2)(returnObject as Vector2?) : Vector2.zero;
            Vector2Field field = new Vector2Field();
            field.value = value;
            field.RegisterCallback<ChangeEvent<Vector2>>(evt => SetValue(key, evt.newValue));
            objectParent.Add(field);
        }
        else if (parameter.ParameterType == typeof(Vector3))
        {
            Vector3 value = (returnObject as Vector3?).HasValue ? (Vector3)(returnObject as Vector3?) : Vector3.zero;
            Vector3Field field = new Vector3Field();
            field.value = value;
            field.RegisterCallback<ChangeEvent<Vector3>>(evt => SetValue(key, evt.newValue));
            objectParent.Add(field);
        }
        else if (parameter.ParameterType == typeof(Vector4))
        {
            Vector4 value = (returnObject as Vector4?).HasValue ? (Vector4)(returnObject as Vector4?) : Vector4.zero;
            Vector4Field field = new Vector4Field();
            field.value = value;
            field.RegisterCallback<ChangeEvent<Vector4>>(evt => SetValue(key, evt.newValue));
            objectParent.Add(field);
        }
        else if (parameter.ParameterType == typeof(Rect))
        {
            Rect value = (returnObject as Rect?).HasValue ? (Rect)(returnObject as Rect?) : Rect.zero;
            RectField field = new RectField();
            field.value = value;
            field.RegisterCallback<ChangeEvent<Rect>>(evt => SetValue(key, evt.newValue));
            objectParent.Add(field);
        }
        else if (parameter.ParameterType == typeof(Bounds))
        {
            Bounds value = (returnObject as Bounds?).HasValue ? (Bounds)(returnObject as Bounds?) : default;
            BoundsField field = new BoundsField();
            field.value = value;
            field.RegisterCallback<ChangeEvent<Bounds>>(evt => SetValue(key, evt.newValue));
            objectParent.Add(field); 
        }
        else if (parameter.ParameterType.IsAssignableFrom(typeof(Object)) || parameter.ParameterType.IsSubclassOf(typeof(Object)))
        {
            ObjectField field = new ObjectField(label);
            field.objectType = parameter.ParameterType;
            field.value = (Object)returnObject;
            field.RegisterCallback<ChangeEvent<Object>>((evt) => SetValue(key,evt.newValue));
            objectParent.Add(field);
        }
        else
        {
            Label unsupported = new Label($"{parameter.ParameterType} is an unsupported type");
            objectParent.Add(unsupported);
        }
    }
    private void SetValue(string key, object value)
    {
        methodParameters[key] = value;
        data.Save();
    }

}
