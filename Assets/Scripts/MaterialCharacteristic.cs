using GridSystem;
using UnityEngine;
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

        private int StateToId()
        {
            int id = 0;
            switch (state)
            {
                case State.Liquid:
                case State.Gas:
                case State.Dust:
                case State.Mud:
                    id = 1;
                    break;
            }
            return id;
        }

        public void Expose(ExposureType exposure)
        {
            //after state changes, refresh the id
            switch (exposure)
            {
                case ExposureType.Heat:
                    HeatExposure();
                    break;
                case ExposureType.Water:
                    WaterExposure();
                    break;
                case ExposureType.Pressure:
                    PressureExposure();
                    break;
            }
            Piece.RefreshId();
        }

        private void HeatExposure()
        {
            switch (state)
            {
                case State.Solid:
                    ChangeState(State.Dust, ExposureType.Heat);
                    break;
                case State.Liquid:
                    break;
                case State.Gas:
                    break;
                case State.Dust:
                    ChangeState(State.Gas, ExposureType.Heat);
                    break;
                case State.Mud:
                    break;
            }
        }
        private void WaterExposure()
        {
            switch (state)
            {
                case State.Solid:
                    break;
                case State.Liquid:
                    break;
                case State.Gas:
                    break;
                case State.Dust:
                    break;
                case State.Mud:
                    break;
            }
        }
        private void PressureExposure()
        {
            switch (state)
            {
                case State.Solid:
                    break;
                case State.Liquid:
                    break;
                case State.Gas:
                    break;
                case State.Dust:
                    break;
                case State.Mud:
                    break;
            }
        }

        private void ChangeState(State newStage, ExposureType? changeSource = null)
        {
            Debug.Log($"{state} to {newStage}; source: {changeSource?.ToString() ?? "unknown"}");
            state = newStage;
            Piece.RefreshId();
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
