using System;
using InputSystemHelper;
using SpellCasting;
using UnityEngine;
using Input = InputSystemHelper.Input;

[RequireComponent(typeof(Movement))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Movement movement;
    
    private void Awake()
    {
        movement ??= GetComponent<Movement>();
        var move = Input.Map("Player").Action("Move");
        move.performed += context =>
        {
            movement.input = context.ReadValue<Vector2>();
            movement.MoveNow();
        };
        move.canceled += _ => movement.input = Vector2.zero;
        Input.Map("Player").Action("Jump").performed += _ => TestSpell();
    }

    private void TestSpell()
    {
        var spell = new FireSigil().Create();
        new MoveSign().Modify(spell);
        spell.target = gameObject;
        spell.Activate(transform.position);
    }

    public class MoveSign : ISigil, ISign
    {
        public Spell Create()
        {
            var spell = new Spell();
            spell.OnActivate += s => s.target = GridManager.GetElement(s.origin);
            spell.OnActivate += Move;
            return spell;
        }
        public void Modify(Spell spell)
        {
            spell.OnActivate += Move;
        }

        private void Move(Spell spell)
        {
            spell.target?.transform.Translate(Vector3.right);
        }
    }
    
    public class FireSigil : ISigil
    {
        public Spell Create()
        {
            var spell = new Spell();
            spell.OnActivate += s =>
            {
                var fire = Resources.Load<GameObject>("fire");
                s.target = Instantiate(fire, s.origin, Quaternion.identity);
            };
            return spell;
        }
    }
}
