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
            delay.ListenNextFinishedCallback(() =>
            {
                if (!jump.inProgress) return;
                spellWindow.Open();
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

    private void TestSpell()
    {
        spellWindow.Close();
        var spell = new FireSigil().Create();
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
        
        spell.Direction = movement.input;
        new MoveSign(Vector2.left).Modify(spell);
        spell.target = this;
        spell.Activate(transform.position);
    }

    public class MoveSign : ISigil, IDirectionalSign
    {
        public Vector2 Direction { get; set; }

        public MoveSign(Vector2? direction = null) => Direction = direction ?? Vector2.up;

        public Spell Create()
        {
            var spell = new Spell();
            spell.OnActivate += s => s.target = IGrid.Instance.GetTileAt(spell.origin).GetPiece();
            spell.OnActivate += Move;
            return spell;
        }
        public void Modify(Spell spell)
        {
            spell.OnActivate += Move;
        }

        private void Move(Spell spell)
        {
            var position = spell.target.coordinate + IDirectionalSign.GetRelativeDirection(this, spell);
            var current = IGrid.Instance.GetTileAt(spell.target.coordinate);
            var target = IGrid.Instance.GetTileAt(position);
            if (target == null) return;
            spell.target?.SetCurrentTile(current, target, position);
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
                s.target = new ObjectPiece(Instantiate(fire, s.origin, Quaternion.identity));
            };
            return spell;
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
