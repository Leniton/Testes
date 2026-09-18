using GridSystem;
namespace GameData
{
    public class MaterialCharacteristic : ICharacteristic
    {
        public IPiece Piece { get; private set; }
        public State state { get; private set; }
        public int idModifier => StateToId();

        public MaterialCharacteristic(State startingState = State.Solid)
        {
            state = startingState;
        }
        
        public void SetUp(IPiece _piece) => Piece = _piece;

        public void Expose(ExposureType exposure)
        {
            //after state changes, refresh the id
            Piece.RefreshId();
        }

        private int StateToId()
        {
            return 0;
        }
        
        public enum ExposureType
        {
            Heat,
            Water,
            Pressure,
        }
        
        public enum State
        {
            Solid,
            Liquid,
            Gas,
            Dust,
            Mud,
        }
    }
}
