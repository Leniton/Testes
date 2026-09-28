using System;
using UnityEngine;
using UnityEngine.UIElements;
namespace GameData.UI
{
    [DefaultExecutionOrder(-99)]
    public class UiController : MonoBehaviour
    {
        public static UiController instance { get; private set; }

        [SerializeField] private UIDocument uiDocument;

        public VisualElement root { get; private set; }

        private void Awake()
        {
            uiDocument ??= GetComponent<UIDocument>();
            if (uiDocument == null) return;
            root = uiDocument.rootVisualElement;
            root.Add(new VisualElement());
            instance = this;
        }
    }
}
