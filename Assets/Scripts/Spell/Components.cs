using System;
using GridSystem;
using UnityEngine;

namespace SpellCasting
{
    public interface ISigil
    {
        public void Create(Spell spell);
    }
    public interface ISign
    {
        public void Modify(Spell spell);
    }
    public class Spell
    {
        private ISign[] signs = new ISign[8];

        public Coordinate origin;
        public Vector2 Direction = Vector2.up;
        public IPiece target;

        public ISigil sigil;
        public ISign this[int id]
        {
            get { return signs[id]; }
            set { signs[id] = value; }
        }

        public void Activate(Vector2 point)
        {
            origin = point;
            sigil?.Create(this);
            target ??= IGrid.Instance.GetTileAt(origin).GetPiece();
            for (int i = 0; i < signs.Length; i++)
                signs[i]?.Modify(this);
        }
    }

    public interface IDirectionalSign : ISign
    {
        public Vector2 Direction { get; set; }

        public static Vector2 GetRelativeDirection(IDirectionalSign sign, Spell spell)
        {
            var angle = Vector2.SignedAngle(Vector2.up, spell.Direction);
            Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * sign.Direction;
            direction.x = Mathf.Round(direction.x);
            direction.y = Mathf.Round(direction.y);
            return direction;
        }
    }
}
