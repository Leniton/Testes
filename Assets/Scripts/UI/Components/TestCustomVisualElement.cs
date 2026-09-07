using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UI.Components
{
    [UxmlElement]
    public partial class TestCustomVisualElement : VisualElement
    {
        [UxmlAttribute] public bool test { get; set; }

        [UxmlCreateInstanceMethod]
        public static TestCustomVisualElement Create()
        {
            TestCustomVisualElement element = new();
            element.test = true;
            return element;
        }

        private void OrganizeElements()
        {
            
        }
    }
}