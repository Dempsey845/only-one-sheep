using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class Crook : MonoBehaviour
{
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float attackCooldownDuration = 5f;
    [SerializeField] private float chaseDuration = 5f;
    [SerializeField] private Image reloadFillImage;

    private bool canAttack = true;
    private LineRenderer lineRenderer;

    SheepStateMachine stateMachine;

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
        if (PlayerInputManager.Instance.AttackPressed)
        {
            HandleAttack();
        }

        if (!canAttack)
        {
            reloadFillImage.fillAmount += Time.deltaTime / chaseDuration;
        }
    }

    private void HandleAttack()
    {
        if (!canAttack) return;

        float distanceFromSheep = PlayerManager.Instance.GetDistanceBetweenPlayerAndSheep();

        if (distanceFromSheep < attackDistance)
        {
            SheepStateController.Instance.ChasePlayer(chaseDuration);
            StartCoroutine(ShowChaseLine());
            Debug.Log("Sheep in distance");
        }

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
                break;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownDuration);
        canAttack = true;
    }
}
