using GridSystem;

namespace GameData
{
    public class ReferenceTrait<T> : ITrait
    {
        public T Value { get; set; }
        public IPiece Piece { get; private set; }
        public void SetUp(IPiece _piece) => Piece = _piece;
        
        public ReferenceTrait(T value) => Value = value;
    }
}