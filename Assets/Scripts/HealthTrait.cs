using System;
using GameData.UI;
using GridSystem;
using UI.Utils;
using UI.Utils.Builder;
using UI.Utils.IValue;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameData
{
    public class HealthTrait : ITrait
    {
        public IPiece Piece { get; private set; }
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }

        public event Action<int> onDamaged;
        public event Action<int> onHealed;
        public event Action onDeath;

        private VisualElement hpBar;
        private VisualElement changingHpBar;
        private VisualElement currentHpBar;
        private Label hpText;

        private static VisualElement container;

        public HealthTrait(int health)
        {
            MaxHealth = health;
            Health = health;

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
        
        public void SetUp(IPiece _piece)
        {
            if (Piece != null) Piece.onTileChanged -= OnTileChanged;
            Piece = _piece;
            if (Piece == null) return;
            Piece.onTileChanged += OnTileChanged;
            OnTileChanged(null, null);
            UpdateUI();
        }
        
        private void OnTileChanged(ITile current, ITile newTile)
        {
            float tileSize = 108f;
            float x = Piece.coordinate.x * tileSize;
            float y = -(Piece.coordinate.y * tileSize) - 80;
            hpBar.Position(new Vector2(x, y), LengthUnit.Pixel);
        }

        public void Damage(IValue<int> damage)
        {
            int health = Math.Max(Health - damage.GetValue(), 0);
            int difference = Health - health;
            bool changed = health != Health;
            if (!changed) return;
            Health = health;
            UpdateUI();
            onDamaged?.Invoke(difference);
            if (Health <= 0) onDeath?.Invoke();
        }

        public void Heal(IValue<int> heal)
        {
            var health = Math.Min(Health + heal.GetValue(), MaxHealth);
            int difference = Health - health;
            bool changed = health != Health;
            if (!changed) return;
            Health = health;
            UpdateUI();
            onHealed?.Invoke(difference);
        }

        private void UpdateUI()
        {
            hpText.text = $"{Health}/{MaxHealth}";
            currentHpBar.Width((Health / (float)MaxHealth) * 100f, LengthUnit.Percent);
        }
    }
}