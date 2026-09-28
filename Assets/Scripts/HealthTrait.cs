using System;
using GameData.UI;
using GridSystem;
using UI.Utils;
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

        public event Action onDamaged;
        public event Action onHealed;
        public event Action onDeath;

        private VisualElement hpBar;

        public HealthTrait(int health)
        {
            MaxHealth = health;
            Health = health;

            var container = new VisualElement()
                .Size(100, unit: LengthUnit.Percent)
                // .BgColor(Color.white.Transparent(.5f))
                .JustifyContent(Justify.Center)
                .AlignItems(Align.Center)
                ;
            container.Add(hpBar = new VisualElement()
                .BgColor(Color.white)
                .AbsPos()
                .Offset(100)
                .Size(100,30));
            UiController.instance.root.Add(container);
        }
        
        public void SetUp(IPiece _piece)
        {
            if (Piece != null) Piece.onTileChanged -= OnTileChanged;
            Piece = _piece;
            if (Piece == null) return;
            Piece.onTileChanged += OnTileChanged;
            OnTileChanged(null, null);
        }
        private void OnTileChanged(ITile current, ITile newTile)
        {
            float tileSize = 108f;
            float x = Piece.coordinate.x * tileSize;
            float y = -(Piece.coordinate.y * tileSize) + 50;
            hpBar.Position(new Vector2(x, y), LengthUnit.Pixel);
        }

        public void Damage(IValue<int> damage)
        {
            int health = Math.Max(Health - damage.GetValue(), 0);
            bool changed = health != Health;
            if (!changed) return;
            Health = health;
            onDamaged?.Invoke();
            if (Health <= 0) onDeath?.Invoke();
        }

        public void Heal(IValue<int> heal)
        {
            var health = Math.Min(Health + heal.GetValue(), MaxHealth);
            bool changed = health != Health;
            if (!changed) return;
            Health = health;
            onHealed?.Invoke();
        }
    }
}