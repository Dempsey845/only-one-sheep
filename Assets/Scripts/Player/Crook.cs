using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class Crook : MonoBehaviour
{
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float attackCooldownDuration = 5f;
    [SerializeField] private float chaseDuration = 5f;
    [SerializeField] private float idleDurationAfterCancel = 3f;
    [SerializeField] private Image reloadFillImage;

    private bool canAttack = true;
    private bool isChasing = false;
    private LineRenderer lineRenderer;

    SheepStateMachine stateMachine;

    public event Action OnPerformedCrook;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
    }

    private void Start()
    {
        stateMachine = SheepStateController.Instance.GetComponent<SheepStateMachine>();
    }

    private void Update()
    {
        if (PlayerInputManager.Instance.AttackPressed && !isChasing)
        {
            HandleAttack();
        }
        else if (PlayerInputManager.Instance.AttackPressed && isChasing)
        {
            CancelChase();
            SheepStateController.Instance.Idle(idleDurationAfterCancel);
            SheepStateController.Instance.GetComponent<SheepPhysicsNavAgent>().SetTargetPosition(SheepStateController.Instance.transform.position);
        }

        if (!canAttack)
        {
            reloadFillImage.fillAmount += Time.deltaTime / attackCooldownDuration;
        }
    }

    private void HandleAttack()
    {
        if (!canAttack) return;

        float distanceFromSheep = PlayerManager.Instance.GetDistanceBetweenPlayerAndSheep();

        if (distanceFromSheep < attackDistance)
        {
            SheepStateMachine stateMachine = SheepStateController.Instance.GetComponent<SheepStateMachine>();
            ISheepState currentSheepState = stateMachine.GetCurrentState();

            if (currentSheepState is SheepCuriousState || currentSheepState is SheepPanicState) return;

            SheepStateController.Instance.ChasePlayer(chaseDuration);
            isChasing = true;
            StartCoroutine(ShowChaseLine());
        }

        OnPerformedCrook?.Invoke();

        reloadFillImage.fillAmount = 0;
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator ShowChaseLine()
    {
        lineRenderer.enabled = true;

        float elapsed = 0f;
        while (elapsed < chaseDuration)
        {
            Transform player = PlayerManager.Instance.transform;
            Transform sheep = SheepStateController.Instance.transform;

            lineRenderer.SetPosition(0, player.position + Vector3.up * 1.5f); // lift line a bit
            lineRenderer.SetPosition(1, sheep.position + Vector3.up * 1.5f);

            ISheepState currentState = stateMachine.GetCurrentState();

            if (currentState is not SheepFollowState)
            {
                isChasing = false;
                break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        CancelChase();
    }

    private void CancelChase()
    {
        lineRenderer.enabled = false;
        isChasing = false;
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownDuration);
        canAttack = true;
    }
}
