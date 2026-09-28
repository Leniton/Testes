using System;
using System.Collections;
using System.Collections.Generic;
using GridSystem;
using LenixSO.Sequences.Coroutines;
using UI.Utils.IValue;
using UnityEngine;
using UnityEngine.Pool;

namespace GameData
{
    public class DamageZonePiece : MonoBehaviour, IPiece
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
            var piece = this as IPiece;
            piece.AddTrait(new GhostTrait());
        }

        private void OnEnable()
        {
            StartCoroutine(DamageDelay());
        }

        public void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates)
        {
            transform.localPosition = newCoordinates;
        }

        private IEnumerator DamageDelay()
        {
            yield return CoroutineExtensions.DelayCoroutine(1);
            DamagePieces();
            if (pool != null) pool.Release(this);
        }

        private void DamagePieces()
        {
            var pieces = IGrid.Instance.GetTileAt(coordinate)?.GetPiecesWith<HealthTrait>();
            if (pieces == null) return;
            IValue<int> dmg = new Value<int>(1);
            for (int i = 0; i < pieces.Count; i++)
                pieces[i].Damage(dmg);
        }
        
        private class GhostTrait : ITrait
        {
            public IPiece Piece { get; private set; }
            public int idModifier => 1;

            public void SetUp(IPiece _piece)
            {
                Piece = _piece;
            }
        }

        private static ObjectPool<DamageZonePiece> pool;

        private static void CreatePool()
        {
            pool = new ObjectPool<DamageZonePiece>(
                () => Instantiate(Resources.Load<DamageZonePiece>("dmg_zone"))
                , piece => piece.gameObject.SetActive(true)
                , piece =>
                {
                    piece.gameObject.SetActive(false);
                    IPiece.PlacePieceOnTile(piece, null, new Coordinate(-999, -999), IGrid.Instance.GetTileAt(piece.coordinate));
                });
        }
        
        public static void CreateDamageZone(Coordinate point, Area area)
        {
            if (pool == null) CreatePool();
            var coordinates = area.GetCoordinates(point);
            for (int i = 0; i < coordinates.Count; i++)
            {
                var piece = pool.Get();
                var coordinate = coordinates[i];
                IPiece.PlacePieceOnTile(piece, IGrid.Instance.GetTileAt(coordinate), coordinate);
            }
        }
    }
}