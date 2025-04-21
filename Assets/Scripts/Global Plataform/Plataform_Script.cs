using UnityEngine;
using PhysicsHelper;

[RequireComponent(typeof(PhysicsHandler))]
public class Plataform_Script : MonoBehaviour
{
#if UNITY_EDITOR
    public bool testMode = true;
#endif
    public float levelOfControl = 1;//change to float level of control?(0 no control, 1 full control)
    public bool useGravity = true;
    public enum State { idle, walking, jumping }
    [SerializeField] public State state = new State();

    public Vector3 input = Vector3.zero;
    //Movement
    [Header("Walk"), SerializeField] private Movement movement;

    [Header("Jump"), SerializeField] private float controlJumpThreshold;
    [SerializeField] private Jump jump;

    private Jump jumpOverride;
    public Jump Jump
    {
        get { return jumpOverride ?? jump; }
        set 
        {
            value.Initialize(physicsHandler);
            value.CopyFloorData(Jump);
            jumpOverride = value; 
        }
    }

    private Movement movementOverride;
    public Movement Movement
    {
        get { return movementOverride ?? movement; }
        set { movementOverride = value; }
    }

    public PhysicsHandler physicsHandler {  get; private set; }

    //for calculating movement while in moving plataforms
    private PhysicsHandler physicsSurface;
    private bool jumped = false;

    void Awake()
    {
        physicsHandler = GetComponent<PhysicsHandler>();
        jump.Initialize(physicsHandler);
        jump.OnLand += OnLanding;
        movement.Initialize(physicsHandler);
        physicsHandler.CollisionExit += OnLeavingPlataform;
    }

    void FixedUpdate()
    {
#if UNITY_EDITOR
        if (testMode)
        {
            jump.CalculateParameters();
            movement.CalculateParameters();
        }
#endif

        Vector3 xInput = Movement.AdjustToNormal(input * Vector2.right, Jump.floorNormal);
        Vector3 inputVelocity = physicsHandler.Velocity;
        inputVelocity = (inputVelocity * (1 - levelOfControl)) + (Movement.Move(xInput) * levelOfControl);
        inputVelocity.y = physicsHandler.Velocity.y;

        if (levelOfControl >= controlJumpThreshold)
        {
            if (input.y > 0)
            {
                if (Jump.onGround)
                {
                    jumped = true;
                    inputVelocity.y = Jump.JumpValue();
                    input.y = 0;
                }
            }
        }

        if(useGravity) inputVelocity.y -= Jump.GravityForce();

        Vector3 finalVelocity = inputVelocity;
        if (!jumped && jump.onGround && physicsSurface)
        {
            finalVelocity.y = 0;
            Vector3 surfaceVelocity = physicsSurface.Velocity;
            finalVelocity += surfaceVelocity;
        }

        physicsHandler.Velocity = finalVelocity;

        //Update state
        if (!jump.onGround)
        {
            input.y = 0;
            state = State.jumping;
        }
        else
        {
            Vector3 moving = inputVelocity - Vector3.Scale(inputVelocity, jump.orientation);
            if (moving == Vector3.zero)
                state = State.idle;
            else
                state = State.walking;
        }
    }

    private void OnLanding(CollisionData data)
    {
        jumped = false;
        physicsSurface = data.gameObject.GetComponent<PhysicsHandler>();
    }
    private void OnLeavingPlataform(CollisionData data)
    {
        if (jump.onGround) return;
        physicsSurface = null;
    }
}