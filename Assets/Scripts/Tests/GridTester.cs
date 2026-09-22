using System;
using System.Collections.Generic;
using GridSystem;
using UnityEngine;

namespace Tests
{
    [DefaultExecutionOrder(999)]
    public class GridTester : MonoBehaviour
    {
        private void Awake()
        {
            Coordinate point = new(5, 3);
            IPiece.PlacePieceOnTile(new BlankPiece(), IGrid.Instance.GetTileAt(point), point);
            IGrid.Instance.ChooseTileInRange(new(5, 3)
                , Area.Diamond(2)
                , Area.Square(1)
                , (coordinate, coordinate1, area) =>
                {
                    Debug.Log($"{coordinate} | {coordinate1}");
                    IGrid.Instance.StopSelecting();
                }
                , -1
                , 1);
        }
    }
    public class BlankPiece : IPiece
    {
        public Action onEnter
        {
            get;
            set;
        }
        public Action onExit
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public int id { get; set; } = 2;
        public Coordinate coordinate
        {
            get;
            set;
        }
        public Action onClick
        {
            get;
            set;
        }
        public List<ITrait> characteristics
        {
            get;
            set;
        }
        public void StylePiece(Sprite sprite, Color color)
        {
            throw new NotImplementedException();
        }
        public void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates) { }
    }
}
