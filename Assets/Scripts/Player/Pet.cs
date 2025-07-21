using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class Pet : MonoBehaviour
{
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float attackCooldownDuration = 5f;
    [SerializeField] private float chaseDuration = 5f;
    [SerializeField] private Image reloadFillImage;

    private bool canAttack = true;
    private bool isChasing = false;

    private SheepStateMachine stateMachine;
    private PlayerActionManager playerActionManager;

    public event Action OnPerformedPet;

    private void Start()
    {
        stateMachine = SheepStateController.Instance.GetComponent<SheepStateMachine>();
        playerActionManager = GetComponent<PlayerActionManager>();
    }

    private void Update()
    {
        if (PlayerInputManager.Instance.SecondaryPressed && !isChasing && !playerActionManager.IsPerformingAction)
        {
            HandleAttack();
        }
       
        if (isChasing)
        {
            ISheepState currentState = stateMachine.GetCurrentState();

            if (currentState is not SheepFollowState)
            {
                isChasing = false;
            }
        }

        if (!canAttack)
        {
            reloadFillImage.fillAmount += Time.deltaTime / attackCooldownDuration;
        }
    }

    private void HandleAttack()
    {
        if (!canAttack) return;

        playerActionManager.StartAction();

        if (SheepManager.Instance != null)
        {
            float distanceFromSheep = PlayerManager.Instance.GetDistanceBetweenPlayerAndSheep();

            if (distanceFromSheep < attackDistance)
            {
                ISheepState currentSheepState = SheepManager.Instance.StateMachine.GetCurrentState();

                if (currentSheepState is SheepCuriousState || currentSheepState is SheepPanicState || currentSheepState is SheepDraggedState) return;

                SheepStateController.Instance.ChasePlayer(chaseDuration);
                isChasing = true;

                SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Love);
            }
        }

        OnPerformedPet?.Invoke();

        reloadFillImage.fillAmount = 0;
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownDuration);
        canAttack = true;
    }
}
