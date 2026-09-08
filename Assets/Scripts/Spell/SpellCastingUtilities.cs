using UnityEngine;

namespace SpellCasting
{
    public static class SpellCastingUtilities
    {
        public static Spell CreateSpell(ISigil sigil, params ISign[] signs)
        {
            var spell = sigil.Create();
            for (int i = 0; i < signs.Length; i++)
                signs[i].Modify(spell);
            return spell;
        }

        public static void CastSpell(Vector2 point, ISigil sigil, params ISign[] signs) => CreateSpell(sigil, signs).Activate(point);
    }
}
