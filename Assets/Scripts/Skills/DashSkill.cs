using System;
using GridSystem;

namespace GameData.Skills
{
    public class DashSkill : Skill
    {
        private MovableTrait move;
        
        private int distance;
        private Action<Coordinate> onDashStart;
        private Action<Coordinate> onDashEnd;

        public DashSkill(int distance = 1, Action<Coordinate> onDashStart = null, Action<Coordinate> onDashEnd = null)
        {
            this.distance = distance;
            this.onDashStart = onDashStart;
            this.onDashEnd = onDashEnd;
        }
        
        public override void Use()
        {
        }

        private void Teleport()
        {
            var target = user.coordinate + (direction * distance);
            var targetTile = IGrid.Instance.GetTileAt(target);
            if ((targetTile.pieceID & ITile.EMPTY) == 0) return;
            var current = IGrid.Instance.GetTileAt(user.coordinate);
            IPiece.PlacePieceOnTile(user, targetTile, target, current);
            onDashEnd?.Invoke(target);
        }

        private void Dash()
        {
            var pos = move.TryMove(direction * distance).finalPosition;
            onDashEnd?.Invoke(pos);
        }

        public override void Cancel()
        {
            move ??= user.GetTrait<MovableTrait>();
            onDashStart?.Invoke(user.coordinate);
            if (move == null) Teleport();
            else Dash();
        }
    }
}