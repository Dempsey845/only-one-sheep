using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector3 jumpFeetPosition;

    private PlayerMovement playerMovement;

    private Vector3 startFeetPosition;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        playerMovement.OnJump += HandleJump;

        startFeetPosition = groundCheckPoint.transform.localPosition;
    }

    private void OnDestroy()
    {
        if (playerMovement != null)
            playerMovement.OnJump -= HandleJump;
    }

    private void Update()
    {
        float speed = playerMovement.IsSprinting ? 1f : playerMovement.GetMoveDirection().magnitude;
        playerAnimator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);

        playerAnimator.SetBool("IsGrounded", playerMovement.IsGrounded);
    }

    private void HandleJump()
    {
        playerAnimator.SetTrigger("IsJumping");
    }

    public void OnJump()
    {
        playerMovement.AddJumpForce();
    }

    public void OnLand()
    {
        StartCoroutine(playerMovement.StartMoveDelay(.7f));
    }
}
