using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Vector3 jumpFeetPosition;

    private PlayerMovement playerMovement;
    private Pet crook;
    private Whistle whistle;
    private Axe axe;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        crook = GetComponent<Pet>();
        whistle = GetComponent<Whistle>();

        if (TryGetComponent(out axe))
        {
            axe.OnPerformedAxe += HandleAxe;
        }

        playerMovement.OnJump += HandleJump;
        crook.OnPerformedPet += HandlePet;
        whistle.OnPerformedWhistle += HandleWhistle;
    }

    private void HandleAxe()
    {
        playerAnimator.SetTrigger("Axe");
    }

    private void HandleWhistle()
    {
        playerAnimator.SetTrigger("Whistle");
    }

    private void HandlePet()
    {
        playerAnimator.SetTrigger("Pet");
    }

    private void OnDestroy()
    {
        if (playerMovement != null)
            playerMovement.OnJump -= HandleJump;
        if (crook != null)
            crook.OnPerformedPet -= HandlePet;
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
