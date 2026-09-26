using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
using GridSystem;
using InputSystemHelper;
using LenixSO.Sequences;
using LenixSO.Sequences.Coroutines;
using UnityEngine;
using UnityEngine.InputSystem;
using Input = InputSystemHelper.Input;

[RequireComponent(typeof(Movement))]
public class PlayerInput : MonoBehaviour, IPiece
{
    [SerializeField] private Movement movement;
    
    public Action onEnter { get; set; }
    public Action onExit { get; set; }
    public string Name { get; set; }
    public int id { get; set; }
    public Coordinate coordinate { get; set; }
    public Action onClick { get; set; }
    public List<ITrait> traits { get; set; }
    
    private ISequence castSequence;
    
    private void Awake()
    {
        movement ??= GetComponent<Movement>();
        movement.piece = this;
        movement.piece.Initialize();
        movement.piece.AddTrait(new MovableTrait());
        
        var move = Input.Map("Player").Action("Move");
        move.performed += OnMovePerformed;
        move.canceled += OnMoveCanceled;
        // Input.Map("Player").Action("Jump").performed += _ => TestSpell();
        var jump = Input.Map("Player").Action("Jump");
        var delay = new CoroutineSequence(new(() => CoroutineExtensions.DelayCoroutine(.2f)));
        castSequence = CustomSequence.EmptySequence();
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
    
    public class ObjectPiece : IPiece
    {
        public Action onEnter { get; set; }
        public Action onExit { get; set; }
        public string Name { get; set; }
        public int id { get; set; }
        public Coordinate coordinate { get; set; }
        public Action onClick { get; set; }
        public List<ITrait> traits { get; set; } = new();

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
}
