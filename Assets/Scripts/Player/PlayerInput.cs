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
        move.performed += context => movement.MoveNow(context.ReadValue<Vector2>());
        move.canceled += _ => movement.ResetMovement();
        Input.Map("Player").Action("Jump").performed += _ => TestSpell();
    }

    private void TestSpell()
    {
        var spell = new FireSigil().Create();
        spell.Direction = movement.input;
        new MoveSign(Vector2.left).Modify(spell);
        spell.target = gameObject;
        spell.Activate(transform.position);
    }

    public class MoveSign : ISigil, IDirectionalSign
    {
        public Vector2 Direction { get; set; }

        public MoveSign(Vector2? direction = null) => Direction = direction ?? Vector2.up;

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
            spell.target?.transform.Translate(IDirectionalSign.GetRelativeDirection(this, spell));
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
