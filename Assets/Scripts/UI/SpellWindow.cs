using System;
using UI.Utils;
using UnityEngine;
using UnityEngine.UIElements;

public class SpellWindow : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private VisualElement root;
    
    private void Awake()
    {
        uiDocument ??= GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        Setup();
    }

    private void Setup()
    {
        root.Add(new VisualElement()
            .FlexDirection(FlexDirection.Row)
            .JustifyContent(Justify.Center)
            .AlignItems(Align.Center)
            //spell view
            .AddElement(new VisualElement()
                .AlignItems(Align.Center)
                .JustifyContent(Justify.Center)
                .AddElement(new UI.Components.CircleLayoutGroup()
                    .AddElement(new VisualElement().Size(90).BgColor(Color.black))
                    .AddElement(new VisualElement().Size(90).BgColor(Color.black))
                    .AddElement(new VisualElement().Size(90).BgColor(Color.black))
                    .AddElement(new VisualElement().Size(90).BgColor(Color.black))
                    .AddElement(new VisualElement().Size(90).BgColor(Color.black))
                    .AddElement(new VisualElement().Size(90).BgColor(Color.black))
                    .AddElement(new VisualElement().Size(90).BgColor(Color.black))
                    .AddElement(new VisualElement().Size(90).BgColor(Color.black))
                    .SetRadius(300))
                .AddElement(new VisualElement()
                    .AbsPos()
                    .LayoutOffset(50, unit: LengthUnit.Percent)
                    .Position(-50)
                    .BgColor(Color.black)
                    .Size(200))
                .Size(900)
                .BgColor(ColorExtension.GrayShade(.6f)))
            //side list (not here probably, this one is just for viewing
            // .AddElement(new ScrollView()
            //     .AbsPos()
            //     .Align(Align.FlexEnd)
            //     .LayoutOffset(2, 50, unit: LengthUnit.Percent)
            //     .Position(0, -50)
            //     .BgColor(Color.white)
            //     .Size(450,900))
            .Size(100, unit: LengthUnit.Percent)
            .BgColor(Color.black.Transparent(.6f)));
        Close();
    }

    public void Open()
    {
        root.Display(DisplayStyle.Flex);
    }
    public void Close()
    {
        root.Display(DisplayStyle.None);
    }
}
