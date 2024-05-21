using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace SerializableMethods
{
    public class SerializedUnityObject: ISerializedObject
    {
        public Type[] usedTypes => new Type[] { typeof(UnityEngine.Object) };
        public VisualElement GetElement(string label, object value, Action<object> onValueChanged)
        {
            return new Label("unity Object");
        }
    }
}