
namespace GridSystem
{
    public interface ICharacteristic
    {
        public IPiece Piece { get; }
        public int idModifier => 0;
        public void ModifyID(ref int id) => id |= idModifier;
        public void SetUp(IPiece _piece);
    }
}