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
        new MoveSign(Vector2.right).Modify(spell);
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
            Vector2 direction = Quaternion.AngleAxis(-Vector2.SignedAngle(Direction, spell.Direction), Vector3.forward) * spell.Direction;
            spell.target?.transform.Translate(direction);
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
