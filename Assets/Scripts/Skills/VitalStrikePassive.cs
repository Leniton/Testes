using System.Collections.Generic;
using GridSystem;
using UnityEngine;
namespace GameData.Skills
{
    public class VitalStrikePassive : Passive
    {
        private IPiece currentTarget;
        
        public override void Setup(IPiece source)
        {
            if (user != null) ClearData();
            base.Setup(source);
            user!.onTileChanged += OnPieceMoved;
        }

        private void ClearData()
        {
            if (currentTarget != null) currentTarget.onTileChanged -= OnTargetMoved;
            currentTarget = null;
            user.onTileChanged -= OnPieceMoved;
        }

        private void OnPieceMoved(ITile current, ITile target)
        {
            if (currentTarget != null) return;
            //look for target in area
            Area area = Area.Square(2);
            var coordinates = area.GetCoordinates(user.coordinate);
            List<IPiece> potentialTargets = new();
            for (int i = 0; i < coordinates.Count; i++)
            {
                var pieces = IGrid.Instance.GetTileAt(coordinates[i])?.GetPiecesWith<HealthTrait>();
                if (pieces is not { Count: > 0 }) continue;
                for (int p = 0; p < pieces.Count; p++)
                    if (pieces[p].Piece != user)
                        potentialTargets.Add(pieces[p].Piece);
            }
            if(potentialTargets is not { Count: > 0 }) return;

            IPiece closest = null;
            for (int i = 0; i < potentialTargets.Count; i++)
            {
                if (closest == null)
                {
                    closest = potentialTargets[i];
                    continue;
                }
                if (Coordinate.Distance(user.coordinate, closest.coordinate) <=
                    Coordinate.Distance(user.coordinate, potentialTargets[i].coordinate)) continue;
                closest = potentialTargets[i];
            }
            SetupTarget(closest);
        }

        private void SetupTarget(IPiece target)
        {
            if (target == null) return;
            if (currentTarget != null)
            {
                currentTarget.onTileChanged -= OnTargetMoved;
            }
            currentTarget = target;
            currentTarget.onTileChanged += OnTargetMoved;
            var mark = new VitalMark(currentTarget, Vector2.right);
        }
        
        private void OnTargetMoved(ITile current, ITile target)
        {
            
        }
    }

    public class VitalMark
    {
        private IPiece piece;
        private GameObject markObject;
        public Vector2 markDirection;
        
        public VitalMark(IPiece target, Vector2? direction = null)
        {
            markDirection = direction ?? Vector2.up;
            markObject = Object.Instantiate(Resources.Load<GameObject>("mark"));
            piece = target;
            piece.onTileChanged += PositionMark;
            PositionMark(null, null);
        }

        private void PositionMark(ITile current, ITile target)
        {
            markObject.transform.position = piece.coordinate;
            markObject.transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.up, markDirection), Vector3.forward);
        }
    }
}
