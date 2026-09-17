using System;
using System.Collections.Generic;
using System.Linq;
using GridSystem;
using InputSystemHelper;
using LenixSO.Sequences;
using LenixSO.Sequences.Coroutines;
using SpellCasting;
using UnityEngine;
using Input = InputSystemHelper.Input;

[RequireComponent(typeof(Movement))]
public class PlayerInput : MonoBehaviour, IPiece
{
    [SerializeField] private Movement movement;
    [SerializeField] private SpellWindow spellWindow;
    
    public Action onEnter { get; set; }
    public Action onExit { get; set; }
    public string Name { get; set; }
    public int id { get; set; }
    public Coordinate coordinate { get; set; }
    public Action onClick { get; set; }
    public List<Characteristic> characteristics { get; set; }
    
    private ISequence castSequence;

    private Spell spell;
    
    private void Awake()
    {
        movement ??= GetComponent<Movement>();
        movement.piece = this;
        movement.piece.Initialize();
        var move = Input.Map("Player").Action("Move");
        move.performed += context => movement.MoveNow(context.ReadValue<Vector2>());
        move.canceled += _ => movement.ResetMovement();
        // Input.Map("Player").Action("Jump").performed += _ => TestSpell();
        var jump = Input.Map("Player").Action("Jump");
        var delay = new CoroutineSequence(new(() => CoroutineExtensions.DelayCoroutine(.2f)));
        castSequence = new CustomSequence(()=>
        {
            CreateSpell();
            delay.ListenNextFinishedCallback(() =>
            {
                if (!jump.inProgress) return;
                spellWindow.Open(spell);
            });
            delay.Begin();
        }, delay.End);
        castSequence.OnFinished += TestSpell;
        jump.performed += _ => castSequence.Begin();
        jump.canceled += _ => castSequence.End();
    }
    
    public void StylePiece(Sprite sprite, Color color) { }
    public void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates)
    {
        // Debug.Log($"{previousTile == null} | {newTile == null} | {newCoordinates}");
        previousTile?.RemovePiece(this);
        newTile?.PlacePiece(this);
        coordinate = newCoordinates;
        transform.localPosition = newCoordinates;
    }

    private void CreateSpell()
    {
        spell = new Spell { 
            sigil = new FireSigil(),
            [0] = new MoveSign(Vector2.right),
            [1] = new MoveSign(),
            [3] = new MoveSign(Vector2.left),
        };
        var signs = new List<ISign>(8);
        //signs.Add(new MoveSign());
        //spell.PositionSigns(signs.ToArray());
        //signs.Add(new MoveSign());
        //spell.PositionSigns(signs.ToArray());
        //signs.Add(new MoveSign());
        //spell.PositionSigns(signs.ToArray());
        //signs.Add(new MoveSign());
        //spell.PositionSigns(signs.ToArray());
        //signs.Add(new MoveSign());
        //spell.PositionSigns(signs.ToArray());
        //signs.Add(new MoveSign());
        //spell.PositionSigns(signs.ToArray());
        //signs.Add(new MoveSign());
        //spell.PositionSigns(signs.ToArray());
        //signs.Add(new MoveSign());
        //spell.PositionSigns(signs.ToArray());
    }
    
    private void TestSpell()
    {
        spellWindow.Close();
        spell.Direction = movement.input;
        spell.Activate(transform.position);
    }

    public class MoveSign : ISigil, IDirectionalSign
    {
        public Vector2 Direction { get; set; }

        public MoveSign(Vector2? direction = null) => Direction = direction ?? Vector2.up;

        public void Create(Spell spell)
        {
            spell.target = IGrid.Instance.GetTileAt(spell.origin).GetPiece();
            Move(spell);
        }
        public void Modify(Spell spell)
        {
            Move(spell);
        }

        private void Move(Spell spell)
        {
            if (spell.target == null) return;
            var position = spell.target.coordinate + IDirectionalSign.GetRelativeDirection(this, spell);
            var current = IGrid.Instance.GetTileAt(spell.target.coordinate);
            var target = IGrid.Instance.GetTileAt(position);
            if (target == null) return;
            spell.target?.SetCurrentTile(current, target, position);
        }
    }
    
    public class FireSigil : ISigil
    {
        public void Create(Spell spell)
        {
            var fire = Resources.Load<GameObject>("fire");
            spell.target = new ObjectPiece(Instantiate(fire, spell.origin, Quaternion.identity));
        }
    }
    
    public class ObjectPiece : IPiece
    {
        public Action onEnter { get; set; }
        public Action onExit { get; set; }
        public string Name { get; set; }
        public int id { get; set; }
        public Coordinate coordinate { get; set; }
        public Action onClick { get; set; }
        public List<Characteristic> characteristics { get; set; } = new();
        public void StylePiece(Sprite sprite, Color color) { }
        
        private GameObject target;
        
        public ObjectPiece(GameObject target)
        {
            this.target = target;
            coordinate = target.transform.localPosition;
            var piece = this as IPiece;
            piece?.Initialize();
        }

        public void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates)
        {
            previousTile?.RemovePiece(this);
            newTile?.PlacePiece(this);
            coordinate = newCoordinates;
            target.transform.localPosition = newCoordinates;
        }
    }
}
