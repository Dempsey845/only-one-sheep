using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.2f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool isGrounded;

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
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);

        // Jump
        if (PlayerInputManager.Instance.JumpPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Physics-based movement
        Vector3 targetVelocity = moveDirection * moveSpeed;
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

    }
}
