using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jumpDelay = 4f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private float fallGravityMultiplier = 3f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;

    private Rigidbody rb;
    private Vector3 moveDirection;

    private bool canJump = true;
    private bool canMove = true;

    private float fallTimer = 0f;
    private const float fallThreshold = -1f;
    private const float fallTime = 0.25f;

    public bool IsSprinting { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsFalling { get; private set; }

    public event Action OnJump;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float h = PlayerInputManager.Instance.MoveInput.x;
        float v = PlayerInputManager.Instance.MoveInput.y;

        // Direction relative to camera
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = right.y = 0f;
        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * v + right * h).normalized;

        // Ground check using Raycast
        IsGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);

        if (!IsGrounded && rb.linearVelocity.y < fallThreshold)
        {
            fallTimer += Time.deltaTime;

            if (fallTimer >= fallTime)
            {
                IsFalling = true;
            }
        }
        else
        {
            IsFalling = false;
            fallTimer = 0f;
        }

        // Jump
        if (canJump && PlayerInputManager.Instance.JumpPressed && IsGrounded)
        {
            OnJump?.Invoke();
            StartCoroutine(JumpDelay());
        }
    }

    private void FixedUpdate()
    {
        if (!canMove) { return; }

        float currentSpeed = moveSpeed;
        if (PlayerInputManager.Instance.SprintPressed && moveDirection.magnitude > 0.1f)
        {
            currentSpeed *= sprintMultiplier;
            IsSprinting = true;
        }
        else
        {
            IsSprinting = false;
        }

        // Physics-based movement
        Vector3 targetVelocity = moveDirection * currentSpeed;
        Vector3 velocityChange = targetVelocity - rb.linearVelocity;
        velocityChange.y = 0; // Preserve vertical velocity (jump/gravity)
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
        

        // Rotate character to face move direction
        if (moveDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion lookRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            transform.rotation = lookRotation;
        }

        if (!IsGrounded)
        {
            float multiplier = rb.linearVelocity.y < 0 ? fallGravityMultiplier : gravityMultiplier;
            rb.AddForce(Physics.gravity * (multiplier - 1f), ForceMode.Acceleration);
        }
    }

    private IEnumerator JumpDelay()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpDelay);
        canJump = true;
    }

    public IEnumerator StartMoveDelay(float delay)
    {
        canMove = false;
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    public void StopMovement(float duration)
    {
        StartCoroutine(StartMoveDelay(duration));
    }

    public void AddJumpForce()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
}
