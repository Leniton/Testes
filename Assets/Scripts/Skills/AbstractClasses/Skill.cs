using GridSystem;
using UnityEngine;

namespace GameData.Skills
{
    public abstract class Skill
    {
        public IPiece user { get; protected set; }
        public Vector2 direction;
        
        public virtual void Setup(IPiece source) => user = source;
        
        public abstract void Use();
        public abstract void Cancel();
    }
}
