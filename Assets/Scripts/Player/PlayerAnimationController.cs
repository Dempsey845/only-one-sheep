using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector3 jumpFeetPosition;

    private PlayerMovement playerMovement;
    private Crook crook;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        crook = GetComponent<Crook>();

        playerMovement.OnJump += HandleJump;
        crook.OnPerformedCrook += HandleCrook;
    }

    private void HandleCrook()
    {
        playerAnimator.SetTrigger("Pet");
    }

    private void OnDestroy()
    {
        if (playerMovement != null)
            playerMovement.OnJump -= HandleJump;
        if (crook != null)
            crook.OnPerformedCrook -= HandleCrook;
    }

    private void Update()
    {
        float speed = playerMovement.IsSprinting ? 1f : playerMovement.GetMoveDirection().magnitude / 2;
        playerAnimator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);

        playerAnimator.SetBool("IsGrounded", playerMovement.IsGrounded);
        playerAnimator.SetBool("IsFalling", playerMovement.IsFalling);
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
