using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SerializableMethods
{
    public static class SerializeMethodHelper
    {
        private const string ReturnValue = "Return value";
        public static string RootPath
        {
            get
            {
                var g = AssetDatabase.FindAssets ( $"t:Script {nameof(SerializeMethodHelper)}" );
                return AssetDatabase.GUIDToAssetPath ( g [ 0 ] );
            }
        }

        private static Dictionary<Type, ISerializedObject> knownTypes;

        public static Dictionary<Type, ISerializedObject> KnownTypes
        {
            get
            {
                if (knownTypes == null)
                {
                    knownTypes = new();
                    LoadKnownTypes();
                }

                return knownTypes;
            }
        }

        public static MethodInfo[] GetMethods(Type targetClass,
            BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        => targetClass.GetMethods(flags);

        public static string MethodKey(MethodInfo method) => method.ToString();
        public static string ParameterKey(MethodInfo method, ParameterInfo parameter) =>
            $"{MethodKey(method)} - {parameter.Name}";

        static MethodTestData data = new();

        public static Dictionary<string, object> methodParameters => data.methodParameters;

        private static void SetValue(string key, object value)
        {
            methodParameters[key] = value;
            data.Save();
        }

        public static void SaveData() => data.Save();

        public static void ShowMethod(GameObject target, MethodInfo method, VisualElement methodsArea)
        {
            KnownTypes.GetType();
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

            if (parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    string key = ParameterKey(method, parameters[i]);
                    if (!methodParameters.ContainsKey(key)) methodParameters.Add(key, null);
                    area.Add(CreateObjectField(method, parameters[i]));
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
                if (methodParameters.ContainsKey(key))
                    returnLabel.text = $"last return: ({methodParameters[key].GetType().Name})[{methodParameters[key]}]";
                area.Add(returnLabel);
            }
        }

        public static VisualElement CreateObjectField(MethodInfo method, ParameterInfo parameter)
        {
            string key = ParameterKey(method, parameter);
            if (!methodParameters.ContainsKey(key)) methodParameters.Add(key, parameter.RawDefaultValue);
            if (methodParameters[key] == null && parameter.RawDefaultValue.GetType() != typeof(DBNull)) methodParameters[key] = parameter.RawDefaultValue;

            string label = parameter.Name;
            object returnObject = methodParameters[key];
            
            Type type = parameter.ParameterType;
            if (!KnownTypes.ContainsKey(type))
            {
                foreach (Type t in KnownTypes.Keys)
                {
                    if (type.IsAssignableFrom(t) || type.IsSubclassOf(t))
                    {
                        return KnownTypes[t].GetElement(label, returnObject, value => SetValue(key, value));
                    }
                }
                return new Label($"{type} is an unsupported type");
            }
            
            //Debug.Log(KnownTypes[type]);
            return KnownTypes[type].GetElement(label, returnObject, value => SetValue(key, value));
            
            if (parameter.ParameterType == typeof(Color))
            {
                Color value = (returnObject as Color?).HasValue ? (Color)(returnObject as Color?) : default;
                ColorField field = new ColorField();
                field.value = value;
                field.RegisterCallback<ChangeEvent<Color>>(evt => SetValue(key, evt.newValue));
                return field;
            }
            else if (parameter.ParameterType == typeof(Vector2))
            {
                Vector2 value = (returnObject as Vector2?).HasValue ? (Vector2)(returnObject as Vector2?) : Vector2.zero;
                Vector2Field field = new Vector2Field();
                field.value = value;
                field.RegisterCallback<ChangeEvent<Vector2>>(evt => SetValue(key, evt.newValue));
                return field;
            }
            else if (parameter.ParameterType == typeof(Vector3))
            {
                Vector3 value = (returnObject as Vector3?).HasValue ? (Vector3)(returnObject as Vector3?) : Vector3.zero;
                Vector3Field field = new Vector3Field();
                field.value = value;
                field.RegisterCallback<ChangeEvent<Vector3>>(evt => SetValue(key, evt.newValue));
                return field;
            }
            else if (parameter.ParameterType == typeof(Vector4))
            {
                Vector4 value = (returnObject as Vector4?).HasValue ? (Vector4)(returnObject as Vector4?) : Vector4.zero;
                Vector4Field field = new Vector4Field();
                field.value = value;
                field.RegisterCallback<ChangeEvent<Vector4>>(evt => SetValue(key, evt.newValue));
                return field;
            }
            else if (parameter.ParameterType == typeof(Rect))
            {
                Rect value = (returnObject as Rect?).HasValue ? (Rect)(returnObject as Rect?) : Rect.zero;
                RectField field = new RectField();
                field.value = value;
                field.RegisterCallback<ChangeEvent<Rect>>(evt => SetValue(key, evt.newValue));
                return field;
            }
            else if (parameter.ParameterType == typeof(Bounds))
            {
                Bounds value = (returnObject as Bounds?).HasValue ? (Bounds)(returnObject as Bounds?) : default;
                BoundsField field = new BoundsField();
                field.value = value;
                field.RegisterCallback<ChangeEvent<Bounds>>(evt => SetValue(key, evt.newValue));
                return field;
            }
            else if (parameter.ParameterType.IsAssignableFrom(typeof(Object)) || parameter.ParameterType.IsSubclassOf(typeof(Object)))
            {
                ObjectField field = new ObjectField(label);
                field.objectType = parameter.ParameterType;
                field.value = (Object)returnObject;
                field.RegisterCallback<ChangeEvent<Object>>((evt) => SetValue(key, evt.newValue));
                return field;
            }
            else
            {
                Label unsupported = new Label($"{parameter.ParameterType} is an unsupported type");
                return unsupported;
            }
        }

        private static void LoadKnownTypes()
        {
            string path = RootPath;
            //Debug.Log(path);
            path = path.Remove(path.IndexOf(nameof(SerializeMethodHelper)));
            path += "SerializedObject/";
            string[] assets = AssetDatabase.FindAssets("", new string[]{path});
            //Debug.Log(path);
            for (int i = 0; i < assets.Length; i++)
            {
                string p = AssetDatabase.GUIDToAssetPath(assets[i]);
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(p);
                ISerializedObject obj = (ISerializedObject)Activator.CreateInstance(script.GetClass());
                foreach (var type in obj.usedTypes)
                {
                    KnownTypes[type] = obj;
                }
            }
        }
    }
}