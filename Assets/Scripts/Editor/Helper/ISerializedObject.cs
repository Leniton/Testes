using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace SerializableMethods
{
    public interface ISerializedObject
    {
        public Type[] usedTypes { get; }
        public VisualElement GetElement(string label, object value, Action<object> onValueChanged);
    }
}