using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class Pet : MonoBehaviour
{
    [SerializeField] private float attackCooldownDuration = 5f;
    [SerializeField] private float chaseDuration = 5f;
    [SerializeField] private Image reloadFillImage;
    [SerializeField] private GameObject petSFXPrefab;

    private bool canPet = true;
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
            HandlePet();
        }
       
        if (isChasing)
        {
            ISheepState currentState = stateMachine.GetCurrentState();

            if (currentState is not SheepFollowState)
            {
                isChasing = false;
            }
        }

        if (!canPet)
        {
            reloadFillImage.fillAmount += Time.deltaTime / attackCooldownDuration;
        }
    }

    private void HandlePet()
    {
        if (!canPet) return;

        playerActionManager.StartAction();

        OnPerformedPet?.Invoke();

        reloadFillImage.fillAmount = 0;
        StartCoroutine(AttackCooldown());
    }

    public bool TryPetSheep()
    {
        if (isChasing) { return false; }

        ISheepState currentSheepState = SheepManager.Instance.StateMachine.GetCurrentState();

        if (currentSheepState is SheepCuriousState || currentSheepState is SheepPanicState || currentSheepState is SheepDraggedState) return false;

        SheepStateController.Instance.ChasePlayer(chaseDuration);
        isChasing = true;

        SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Love);
        SheepManager.Instance.EnqueueRandomSheepClip();
        Instantiate(petSFXPrefab, transform.position, Quaternion.identity);

        return true;
    }

    private IEnumerator AttackCooldown()
    {
        canPet = false;
        yield return new WaitForSeconds(attackCooldownDuration);
        canPet = true;
    }
}
