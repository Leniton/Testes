using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace SerializableMethods
{
    public class SerializedInt: ISerializedObject
    {
        public Type[] usedTypes => new Type[] { typeof(int) };
        
        public VisualElement GetElement(string label, object value, Action<object> onValueChanged)
        {
            IntegerField field = new IntegerField(label);
            field.value = (int)value;
            field.RegisterCallback<ChangeEvent<int>>(evt => onValueChanged?.Invoke(evt.newValue));
            return field;
        }
    }
}