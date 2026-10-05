using System;
using System.Collections.Generic;
using GridSystem;

namespace GameData.Skills
{
    public class AutoTargetSkill : Skill
    {
        private Skill skill;
        
        public AutoTargetSkill(Skill skill)
        {
            this.skill = skill;
        }

        public override void Setup(IPiece source)
        {
            base.Setup(source);
            if (skill != null) skill.Setup(source);
        }

        public override void Use()
        {
            var target = GetClosestPiece(user.coordinate, Area.Square(1, 1));
            if (target == null) return;
            skill.direction = target.coordinate - user.coordinate;
        }

        public override void Cancel()
        {
            skill.Cancel();
        }

        public static IPiece GetClosestPiece(Coordinate coordinate, Area? detectionArea = null, Func<IPiece,bool> criteria = null)
        {
            Area area = detectionArea ?? Area.Square(2);
            var coordinates = area.GetCoordinates(coordinate);
            List<IPiece> potentialTargets = new();
            for (int i = 0; i < coordinates.Count; i++)
            {
                var tile = IGrid.Instance.GetTileAt(coordinates[i]);
                for (int j = 0; j < tile.pieces.Count; j++)
                    if (criteria?.Invoke(tile.pieces[j]) ?? true)
                        potentialTargets.Add(tile.pieces[j]);

            }

            if(potentialTargets is not { Count: > 0 }) return null;

            IPiece closest = potentialTargets[0];
            for (int i = 1; i < potentialTargets.Count; i++)
            {
                if (Coordinate.Distance(coordinate, closest.coordinate) <=
                    Coordinate.Distance(coordinate, potentialTargets[i].coordinate)) continue;
                closest = potentialTargets[i];
            }
            return closest;
        }
    }
}