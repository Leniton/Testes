using GridSystem;

namespace GameData.Skills
{
    public abstract class Passive
    {
        public IPiece user { get; protected set; }
        public virtual void Setup(IPiece source) => user = source;
    }
}
