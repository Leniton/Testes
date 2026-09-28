using System;
using System.Collections.Generic;
using GridSystem;
using LenixSO.Sequences.Coroutines;
using UI.Utils.IValue;
using UnityEngine;

namespace GameData
{
    public class DummyPiece : MonoBehaviour, IPiece
    {
        public Action onEnter { get; set; }
        public Action onExit { get; set; }
        public string Name { get; set; }
        public int id { get; set; }
        public Coordinate coordinate { get; set; }
        public Action onClick { get; set; }
        public List<ITrait> traits { get; set; }
        public Action<ITile, ITile> onTileChanged { get; set; }

        private void Awake()
        {
            IPiece piece = this;
            HealthTrait healthTrait = new(50);
            piece.AddTrait(healthTrait);
            healthTrait.onDamaged += () =>
                CoroutineExtensions.AwaitCoroutine(CoroutineExtensions.DelayCoroutine(1), 
                    () => healthTrait.Heal(new Value<int>(healthTrait.MaxHealth)));
        }

        public void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates)
        {
            transform.localPosition = newCoordinates;
        }
    }
}
