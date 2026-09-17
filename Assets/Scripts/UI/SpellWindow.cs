using System;
using System.Collections.Generic;
using SpellCasting;
using UI.Utils;
using UI.Utils.Builder;
using UnityEngine;
using UnityEngine.UIElements;

public class SpellWindow : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private VisualElement root;
    private VisualElement spellContainer;
    private VisualElement sigil;
    private UI.Components.CircleLayoutGroup signContainer;

    #region SpriteReference
    private Dictionary<Type, Sprite> sprites = new();

    private Sprite moveSign => Resources.Load<Sprite>("sign_move");
    private Sprite fireSigil => Resources.Load<Sprite>("sigil_fire");

    private void SetupSpriteDictionary()
    {
        sprites.Clear();
        sprites[typeof(PlayerInput.MoveSign)] = moveSign;
        sprites[typeof(PlayerInput.FireSigil)] = fireSigil;
    }
    #endregion
    
    private void Awake()
    {
        uiDocument ??= GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        Setup();
        SetupSpriteDictionary();
    }

    private void Setup()
    {
        root.Add(new VisualElement()
            .FlexDirection(FlexDirection.Row)
            .JustifyContent(Justify.Center)
            .AlignItems(Align.Center)
            //spell view
            .AddElement(spellContainer = new VisualElement()
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
        SetupSpellView();
    }

    private void SetupSpellView()
    {
        spellContainer.Add(new VisualElement()
            .BgImage(Resources.Load<Sprite>("circle"))
            .Size(96, unit: LengthUnit.Percent)
            .LayoutOffset(50, unit: LengthUnit.Percent)
            .Position(-50)
            .AbsPos());
        spellContainer.Add(spellContainer = new VisualElement()
            .AlignItems(Align.Center)
            .JustifyContent(Justify.Center)
            .Size(96, unit: LengthUnit.Percent)
            .Margin(2, unit: LengthUnit.Percent)
            .AddElement(signContainer = new UI.Components.CircleLayoutGroup()
                .AddElement(new VisualElement().Size(90).BgColor(Color.black).AddElement(new VisualElement().Size(100, unit: LengthUnit.Percent)))
                .AddElement(new VisualElement().Size(90).BgColor(Color.black).AddElement(new VisualElement().Size(100, unit: LengthUnit.Percent)))
                .AddElement(new VisualElement().Size(90).BgColor(Color.black).AddElement(new VisualElement().Size(100, unit: LengthUnit.Percent)))
                .AddElement(new VisualElement().Size(90).BgColor(Color.black).AddElement(new VisualElement().Size(100, unit: LengthUnit.Percent)))
                .AddElement(new VisualElement().Size(90).BgColor(Color.black).AddElement(new VisualElement().Size(100, unit: LengthUnit.Percent)))
                .AddElement(new VisualElement().Size(90).BgColor(Color.black).AddElement(new VisualElement().Size(100, unit: LengthUnit.Percent)))
                .AddElement(new VisualElement().Size(90).BgColor(Color.black).AddElement(new VisualElement().Size(100, unit: LengthUnit.Percent)))
                .AddElement(new VisualElement().Size(90).BgColor(Color.black).AddElement(new VisualElement().Size(100, unit: LengthUnit.Percent)))
                .SetOffset(.5f)
                .SetRotation(UI.Components.Rotation.CounterClockwise)
                .SetRadius(300))
            .AddElement(sigil = new VisualElement()
                .AbsPos()
                .LayoutOffset(50, unit: LengthUnit.Percent)
                .Position(-50)
                .BgColor(Color.black)
                .Size(200))
            );
    }

    public void RenderSpell(Spell spell)
    {
        if (spell == null) return;
        spellContainer.Rotation(Vector2.SignedAngle(spell.Direction, Vector2.up));
        sigil.BgImage(sprites.TryGetValue(spell.sigil.GetType(), out var sprite) ? sprite : null);
        for (int i = 0; i < signContainer.childCount; i++)
        {
            var sign = spell[i];
            var img = signContainer[i][0].BgImage(sign == null ? null : 
                sprites.TryGetValue(sign.GetType(), out sprite) ? sprite : null);
            if (sign is not IDirectionalSign dirSign) continue;
            img.Rotation(Vector2.SignedAngle(dirSign.Direction, Vector2.up));
        }
    }

    public void Open(Spell spell = null)
    {
        RenderSpell(spell);
        root.Display(DisplayStyle.Flex);
    }
    public void Close()
    {
        root.Display(DisplayStyle.None);
    }
}
