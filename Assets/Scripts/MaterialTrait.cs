using System;
using System.Collections.Generic;
using GridSystem;
using UnityEngine;
namespace GameData
{
    public class MaterialTrait : ITrait
    {
        public IPiece Piece { get; private set; }
        public State state { get; private set; }
        public int idModifier => StateToId();

        private Dictionary<ExposureType, Action> exposureReactions = new();

        public MaterialTrait(State startingState = State.Solid)
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
            if (!exposureReactions.TryGetValue(exposure, out Action action)) return;
            action?.Invoke();
        }

        public void RegisterCallback(ExposureType exposureType, Action callback)
        {
            if(!exposureReactions.TryAdd(exposureType, callback))
                exposureReactions[exposureType] += callback;
        }

        public void UnregisterCallback(ExposureType exposureType, Action callback)
        {
            if(exposureReactions.TryGetValue(exposureType, out _))
                exposureReactions[exposureType] -= callback;
        }

        public void ChangeState(State newState, ExposureType? changeSource = null)
        {
            // Debug.Log($"{state} to {newState}; source: {changeSource?.ToString() ?? "unknown"}");
            state = newState;
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
