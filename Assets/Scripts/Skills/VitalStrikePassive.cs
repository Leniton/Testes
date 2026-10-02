using System.Collections.Generic;
using GridSystem;
using UI.Utils.IValue;
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
            currentTarget = null;
            user.onTileChanged -= OnPieceMoved;
        }

        private void OnPieceMoved(ITile current, ITile target)
        {
            if (currentTarget != null) return;
            //look for target in area
            Area area = Area.Square(2);
            var coordinates = area.GetCoordinates(user.coordinate);
            List<HealthTrait> potentialTargets = new();
            for (int i = 0; i < coordinates.Count; i++)
            {
                var pieces = IGrid.Instance.GetTileAt(coordinates[i])?.GetPiecesWith<HealthTrait>();
                if (pieces is not { Count: > 0 }) continue;
                for (int p = 0; p < pieces.Count; p++)
                    if (pieces[p].Piece != user)
                        potentialTargets.Add(pieces[p]);
            }
            if(potentialTargets is not { Count: > 0 }) return;

            HealthTrait closest = null;
            for (int i = 0; i < potentialTargets.Count; i++)
            {
                if (closest == null)
                {
                    closest = potentialTargets[i];
                    continue;
                }
                if (Coordinate.Distance(user.coordinate, closest.Piece.coordinate) <=
                    Coordinate.Distance(user.coordinate, potentialTargets[i].Piece.coordinate)) continue;
                closest = potentialTargets[i];
            }
            SetupTarget(closest);
        }

        private void SetupTarget(HealthTrait target)
        {
            if (target == null) return;
            currentTarget = target.Piece;
            var mark = new VitalMark(user, target);
        }
    }

    public class VitalMark
    {
        private HealthTrait trait;
        private IPiece piece => trait.Piece;
        private IPiece markSource;
        private GameObject markObject;
        public Vector2 markDirection;
        
        public VitalMark(IPiece origin, HealthTrait target, Vector2? direction = null)
        {
            markDirection = direction ?? RandomDirection();
            markSource = origin;
            trait = target;
            markObject = Object.Instantiate(Resources.Load<GameObject>("mark"));
            trait.onDamaged += CheckMarkHit;
            piece.onTileChanged += PositionMark;
            PositionMark(null, null);
        }

        private Vector2 RandomDirection()
        {
            int r = Random.Range(0, 4);
            return Quaternion.AngleAxis(90f * r, Vector3.forward) * Vector3.up;
        }

        private void PositionMark(ITile current, ITile target)
        {
            markObject.transform.position = piece.coordinate;
            markObject.transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.up, markDirection), Vector3.forward);
        }

        private void CheckMarkHit(int diff, IPiece source)
        {
            if (source != markSource) return;
            Vector2 hitDirection = markSource.coordinate - piece.coordinate;
            if (hitDirection.normalized != markDirection) return;
            //detonate mark
            trait.onDamaged -= CheckMarkHit;
            trait.Damage(new Value<int>(5), source);
            trait.onDamaged += CheckMarkHit;
        }
    }
}
