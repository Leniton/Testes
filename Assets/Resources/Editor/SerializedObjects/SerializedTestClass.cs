using System;
using SerializableMethods;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class SerializedTestClass : ISerializedObject
{
    public Type[] usedTypes => new [] { typeof(TestGenericClass) };
    public VisualElement GetElement(string label, object value, Type type, Action<object> onValueChanged)
    {
        return new VisualElement();
        /*ObjectField field = new ObjectField(label);
        field.objectType = type;
        field.value = (Object)value;
        field.RegisterCallback<ChangeEvent<Object>>((evt) => onValueChanged?.Invoke(evt.newValue));
        return field;*/
    }
}