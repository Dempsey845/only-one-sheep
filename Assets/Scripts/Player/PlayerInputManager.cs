using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;

    private PlayerInputActions inputActions;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool WhistlePressed { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool SecondaryPressed { get; private set; }
    public bool SprintPressed { get; private set; }
    public bool MenuPressed { get; private set; }

    private void Awake()
    {
        Instance = this;

        inputActions = new PlayerInputActions();

        // Bind input callbacks
        inputActions.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => MoveInput = Vector2.zero;

        inputActions.Player.Look.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += _ => LookInput = Vector2.zero;

        inputActions.Player.Jump.performed += _ => JumpPressed = true;

        inputActions.Player.Whistle.performed += _ => WhistlePressed = true;

        inputActions.Player.Attack.performed += ctx => AttackPressed = true;

        inputActions.Player.Secondary.performed += ctx => SecondaryPressed = true;

        inputActions.Player.Sprint.performed += ctx => SprintPressed = true;
        inputActions.Player.Sprint.canceled += ctx => SprintPressed = false;

        inputActions.Player.Menu.performed += ctx => MenuPressed = true;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void LateUpdate()
    {
        // Reset one-time inputs
        JumpPressed = false;
        WhistlePressed = false;
        AttackPressed = false;
        SecondaryPressed = false;
        MenuPressed = false;
    }

    public void LockLookInput()
    {
        LookInput = Vector2.zero;
    }
}
