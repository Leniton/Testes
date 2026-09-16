
namespace GridSystem
{
    public abstract class Characteristic
    {
        private IPiece piece;
        public IPiece Piece => piece;
        public virtual int idModifier => 0;
        public int ModifyID(int id) => id |= idModifier;
        public virtual void SetUp(IPiece _piece) => piece = _piece;
    }
}