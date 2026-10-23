using GameData;
using GameData.UI;
using GridSystem;
using UI.Utils;
using UI.Utils.Builder;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameData
{
    public class HealthUi
    {
        private static VisualElement container;

        private HealthTrait hp;
        
        private VisualElement hpBar;
        private VisualElement changingHpBar;
        private VisualElement currentHpBar;
        private Label hpText;
        
        public HealthUi(HealthTrait trait)
        {
            hp = trait;
            hp.onDamaged += UpdateUI;
            hp.onHealed += UpdateUI;
            
            container ??= new VisualElement()
                .Size(100, unit: LengthUnit.Percent)
                // .BgColor(Color.white.Transparent(.5f))
                .JustifyContent(Justify.Center)
                .AlignItems(Align.Center);
            container.Add(hpBar = new VisualElement()
                .AbsPos()
                .BgColor(Color.white)
                .JustifyContent(Justify.Center)
                .AlignItems(Align.Center)
                .Padding(5)
                .Size(100,30));
            hpBar.Add(changingHpBar = new VisualElement()
                .BgColor(Color.darkGreen.Transparent(.6f))
                .Align(Align.FlexStart)
                .Size(100, unit: LengthUnit.Percent));
            changingHpBar.Add(currentHpBar = new VisualElement()
                .AbsPos()
                .BgColor(Color.green)
                .Align(Align.FlexStart)
                .Size(100, unit: LengthUnit.Percent));
            hpBar.Add(hpText = new Label("999/999")
                .AbsPos()
                .FontSize(20)
                .TextAlign(TextAnchor.MiddleCenter)
                .Size(100, unit: LengthUnit.Percent));
            UiController.instance.root.Add(container);
        }

        private void UpdateUI(int delta, IPiece piece)
        {
            hpText.text = $"{hp.Health}/{hp.MaxHealth}";
            currentHpBar.Width((hp.Health / (float)hp.MaxHealth) * 100f, LengthUnit.Percent);
        }
        
        public void PositionUI(Vector2 position)
        {
            float tileSize = 108f;
            position.x *= tileSize;
            position.y = -(position.y * tileSize) - 80;
            hpBar.Position(position, LengthUnit.Pixel);
        }
    }
}