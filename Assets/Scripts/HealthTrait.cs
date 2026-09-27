using System;
using GridSystem;
using UI.Utils.IValue;

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

        public HealthTrait(int health)
        {
            MaxHealth = health;
            Health = health;
        }
        
        public void SetUp(IPiece _piece)
        {
           Piece = _piece;
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