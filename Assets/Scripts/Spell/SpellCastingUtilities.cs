using System;
using UnityEngine;

namespace SpellCasting
{
    public static class SpellCastingUtilities
    {
        private const int maxSigns = 8;
        
        public static Spell PositionSigns(this Spell spell, params ISign[] signs)
        {
            //signs above 8 are ignored
            int signCount = Math.Min(signs.Length, maxSigns);
            int progress = Mathf.CeilToInt(maxSigns / (float)signCount);
            // Debug.Log(progress);
            for (int i = 0; i < signCount; i++)
            {
                int id = i * progress;
                signs[i].Modify(spell);
                if (signs[i] is not IDirectionalSign sign) continue;
                int yOffset = (maxSigns + id + 1) % maxSigns;
                float y = 0;
                if (yOffset < 3) y = 1;
                else if (yOffset is > 3 and < 7) y = -1;
                float x = 0;
                if (i is > 0 and < 4) x = 1;
                else if (i > 4) x = -1;
                // Debug.Log($"{i}({id}) => {x},{y}");
                sign.Direction = new(x, y);
            }

            return spell;
        }

        public static Vector2 IdToDirection(int id)
        {
            int yOffset = (maxSigns + id + 1) % maxSigns;
            float y = 0;
            if (yOffset < 3) y = 1;
            else if (yOffset is > 3 and < 7) y = -1;
            float x = 0;
            if (id is > 0 and < 4) x = 1;
            else if (id > 4) x = -1;
            // Debug.Log($"{i}({id}) => {x},{y}");
            return new(x, y);
        }
    }
}
