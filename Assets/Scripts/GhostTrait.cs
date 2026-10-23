using GridSystem;

namespace GameData
{
    public class GhostTrait : ITrait
    {
        public int idModifier { get; } = ITile.EMPTY;
        public IPiece Piece { get; private set; }
        public void SetUp(IPiece _piece) => Piece = _piece;
    }
}