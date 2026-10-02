using GridSystem;
using UI.Utils.IValue;
using UnityEngine;

namespace GameData.Skills
{
    public class BasicAttackSkill : Skill
    {
        public IValue<int> damage;

        public BasicAttackSkill(IValue<int> dmg = null)
        {
            damage = dmg ??= new Value<int>(1);
        }
        
        public override void Use()
        {
            //feedback?
        }
        
        public override void Cancel()
        {
            if (direction == Vector2.zero) direction = Vector2.up;
            var targetTile = IGrid.Instance.GetTileAt(user.coordinate + direction);
            if (targetTile == null) return;
            var pieces = targetTile.GetPiecesWith<HealthTrait>();
            pieces ??= new();
            for (int i = 0; i < pieces.Count; i++)
                pieces[i].Damage(damage, user);
        }
    }
}
