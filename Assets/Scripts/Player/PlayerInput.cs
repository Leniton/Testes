using System;
using System.Collections.Generic;
using System.Linq;
using GameData;
using GameData.Skills;
using GridSystem;
using InputSystemHelper;
using LenixSO.Sequences;
using LenixSO.Sequences.Coroutines;
using UI.Utils.IValue;
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
    public Action<ITile, ITile> onTileChanged { get; set; }
    
    private ISequence castSequence;
    
    private Skill skill;
    
    private void Awake()
    {
        IPiece.PlacePieceOnTile(this, IGrid.Instance.GetTileAt(transform.position), transform.position);
        movement ??= GetComponent<Movement>();
        movement.piece = this;
        movement.piece.Initialize();
        movement.piece.AddTrait(new MovableTrait());
        var health = new HealthTrait(5);
        movement.piece.AddTrait(health);
        
        var move = Input.Map("Player").Action("Move");
        move.performed += OnMovePerformed;
        move.canceled += OnMoveCanceled;
        var jump = Input.Map("Player").Action("Jump");
        var delay = new CoroutineSequence(new(() => CoroutineExtensions.DelayCoroutine(.2f)));
        castSequence = CustomSequence.EmptySequence();
        castSequence = new CustomSequence(() => DamageZonePiece.CreateDamageZone(coordinate, Area.Circle(2), source:this));
        Passive passive = new VitalStrikePassive();
        passive.Setup(this);
        skill = new BasicAttackSkill(new Value<int>(1));
        skill = new AutoTargetSkill(skill);
        skill.Setup(this);
        var atk = skill;
        skill = new DashSkill(onDashEnd: _ =>
        {
            atk.direction = skill.direction;
            atk.Use();
            atk.Cancel();
        });
        skill.Setup(this);
        var attack = Input.Map("Player").Action("Attack");
        attack.performed += _ => castSequence.Begin();
        attack.canceled += _ => castSequence.End();
        
        jump.performed += _ => skill.Use();
        jump.canceled += _ => skill.Cancel();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        if (direction != Vector2.zero) skill.direction = direction;
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
        public Action<ITile, ITile> onTileChanged { get; set; }

        
        private GameObject target;
        
        public ObjectPiece(GameObject target, Action<ITile,ITile> OnTileChanged = null)
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
        }
    }
}
