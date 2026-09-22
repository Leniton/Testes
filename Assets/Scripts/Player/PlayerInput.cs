using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
using GridSystem;
using InputSystemHelper;
using LenixSO.Sequences;
using LenixSO.Sequences.Coroutines;
using SpellCasting;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public List<ITrait> characteristics { get; set; }
    
    private ISequence castSequence;

    private Spell spell;
    
    private void Awake()
    {
        movement ??= GetComponent<Movement>();
        movement.piece = this;
        movement.piece.Initialize();
        movement.piece.AddCharacteristic(new MovableTrait());
        
        var move = Input.Map("Player").Action("Move");
        move.performed += OnMovePerformed;
        move.canceled += OnMoveCanceled;
        // Input.Map("Player").Action("Jump").performed += _ => TestSpell();
        var jump = Input.Map("Player").Action("Jump");
        var delay = new CoroutineSequence(new(() => CoroutineExtensions.DelayCoroutine(.2f)));
        castSequence = new CustomSequence(()=>
        {
            CreateSpell();
            movement.input = Vector2.zero;
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

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        if (direction == Vector2.zero || !castSequence.running)
        {
            movement.MoveNow(direction);
            return;
        }
        spell.Direction = direction;
        spellWindow.RenderSpell(spell);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        movement.ResetMovement();
    }
    
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
            [0] = new MoveSign(),
            // [1] = new MoveSign(),
            [2] = new BlankSign(),
            [3] = new BlankSign(),
            [4] = new BlankSign(),
            [5] = new BlankSign(),
        };
        spell.Direction = movement.input;
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
        spell.Activate(transform.position);
    }

    public class MoveSign : ISigil, IDirectionalSign
    {
        public Vector2 Direction { get; set; }

        public MoveSign(Vector2? direction = null) => Direction = direction ?? Vector2.up;

        public void Create(Spell spell)
        {
            Direction = Vector2.up;//forced as sigils can't rotate
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
            var direction = IDirectionalSign.GetRelativeDirection(this, spell);
            var current = IGrid.Instance.GetTileAt(spell.target.coordinate);
            var movable = spell.target.GetCharacteristic<MovableTrait>();
            if (movable != null)
            {
                movable.TryMove(direction);
                return;
            }
            var position = spell.target.coordinate + direction;
            var target = IGrid.Instance.GetTileAt(position);
            if (target == null) return;
            IPiece.PlacePieceOnTile(spell.target, target, position, current);
        }
    }
    
    public class FireSigil : ISigil
    {
        public void Create(Spell spell)
        {
            var fire = Resources.Load<GameObject>("fire");
            var piece = new ObjectPiece(Instantiate(fire, spell.origin, Quaternion.identity));
            piece.id = 1;
            piece.onTileChanged += tile =>
            {
                var materials = tile.GetPiecesWith<MaterialTrait>();
                if (materials is not { Count: > 0 }) return;
                for (int i = 0; i < materials.Count; i++)
                    materials[i].Expose(MaterialTrait.ExposureType.Heat);
            };
            spell.target = piece;
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
        public List<ITrait> characteristics { get; set; } = new();

        public event Action<ITile> onTileChanged;
        
        private GameObject target;
        
        public ObjectPiece(GameObject target, Action<ITile> OnTileChanged = null)
        {
            this.target = target;
            coordinate = target.transform.localPosition;
            var piece = this as IPiece;
            onTileChanged += OnTileChanged;
            piece?.Initialize();
        }

        public void SetCurrentTile(ITile previousTile, ITile newTile, Coordinate newCoordinates)
        {
            previousTile?.RemovePiece(this);
            newTile?.PlacePiece(this);
            coordinate = newCoordinates;
            target.transform.localPosition = newCoordinates;
            onTileChanged?.Invoke(newTile);
        }
    }
    
    public class BlankSign : ISign
    {
        public void Modify(Spell spell) { }
    }
}
