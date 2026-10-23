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

        public event Action<int, IPiece> onDamaged;
        public event Action<int, IPiece> onHealed;
        public event Action onDeath;

        public HealthTrait(int health)
        {
            MaxHealth = health;
            Health = health;
        }

        public void SetUp(IPiece _piece) => Piece = _piece;
        
        public void Damage(IValue<int> damage, IPiece source = null)
        {
            int health = Math.Max(Health - damage.GetValue(), 0);
            int difference = Health - health;
            bool changed = health != Health;
            if (!changed) return;
            Health = health;
            onDamaged?.Invoke(difference, source);
            if (Health <= 0) onDeath?.Invoke();
        }

        public void Heal(IValue<int> heal, IPiece source = null)
        {
            var health = Math.Min(Health + heal.GetValue(), MaxHealth);
            int difference = Health - health;
            bool changed = health != Health;
            if (!changed) return;
            Health = health;
            onHealed?.Invoke(difference, source);
        }
    }
}