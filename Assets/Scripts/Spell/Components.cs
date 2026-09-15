using System;
using UnityEngine;

namespace SpellCasting
{
    public interface ISigil
    {
        public Spell Create();
    }
    public interface ISign
    {
        public void Modify(Spell spell);
    }
    public class Spell
    {
        public Vector2 origin;
        public Vector2 Direction = Vector2.up;
        public GameObject target;

        public event Action<Spell> OnActivate; 

        public void Activate(Vector2 point)
        {
            origin = point;
            OnActivate?.Invoke(this);
        }
    }

    public interface IDirectionalSign : ISign
    {
        public Vector2 Direction { get; set; }
    }
}
