using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FoxAgent : MonoBehaviour
{
    [SerializeField] private float targetCheckRate = 1f;
    [SerializeField] private float stopDistance = 2f;

    [Header("Flee")]
    [SerializeField] private float fleeDistance = 20f;
    [SerializeField] private float checkRadius = 5f;

    private NavMeshAgent agent;
    private FoxStateController stateController;

    private Vector3 targetPosition;
    private float timer = 0f;
    private bool chaseSheep = false;
    private float startSpeed;

    private bool hasFleePoint = false;
    private float fleePointTimer = 0f;
    private float fleeTimer = 0f;
    private float predictedTimeToFleePoint;

    private const float FLEE_POINT_TOLERANCE = 1f;
    private const float CHECK_FLEE_RATE = 1f;
    private const float TIME_TO_FLEE_POINT_TOLERANCE = 4f;

    public event Action OnFoxReachedSheepDuringChase;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        stateController = GetComponent<FoxStateController>();
        StopChasingSheep();
        startSpeed = agent.speed;
    }

    private void Update()
    {
        if (SheepManager.Instance == null) { return; }

        if (chaseSheep)
        {
            ChaseSheep();
        }

        if (hasFleePoint)
        {
            Flee();
        }
    }

    private void Flee()
    {
        fleePointTimer += Time.deltaTime;
        fleeTimer += Time.deltaTime;

        if (fleePointTimer >= CHECK_FLEE_RATE)
        {
            if (HasReachedDestination(targetPosition, FLEE_POINT_TOLERANCE))
            {
                CancelFlee();
                stateController.Idle();
            }

            fleePointTimer = 0f;
        }

        if (fleeTimer > predictedTimeToFleePoint)
        {
            CancelFlee();
            stateController.GoHome();
        }
    }

    private void CancelFlee()
    {
        hasFleePoint = false;
        stateController.SetIsFleeing(false);
        fleeTimer = 0f;
    }

    private void ChaseSheep()
    {
        timer += Time.deltaTime;

        if (timer > targetCheckRate)
        {
            targetPosition = SheepManager.Instance.transform.position;
            float distance = Vector3.Distance(transform.position, targetPosition);

            if (distance <= stopDistance)
            {
                agent.SetDestination(transform.position);
                OnFoxReachedSheepDuringChase?.Invoke();
            }
            else
            {
                agent.SetDestination(targetPosition);
            }

            timer = 0f;
        }
    }

    public void StopChasingSheep()
    {
        chaseSheep = false; ;
        agent.SetDestination(transform.position);
    }

    public void StartChasingSheep()
    {
        chaseSheep = true;
    }

    public void GoToDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public bool HasReachedDestination(Vector3 destination, float tolerance = 2f)
    {
        float distance = Vector3.Distance(transform.position, destination);

        return distance <= tolerance;
    }

    public void TryFlee()
    {
        if (PlayerManager.Instance == null) return;

        Vector3 playerPosition = PlayerManager.Instance.GetPosition();
        Vector3 baseDirection = (transform.position - playerPosition).normalized;

        int maxAttempts = 5;
        float angleStep = 30f; 
        float currentAngle = 0f;

        for (int i = 0; i < maxAttempts; i++)
        {
            // Rotate base direction slightly to find alternatives
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * baseDirection;
            Vector3 desiredPosition = transform.position + direction * fleeDistance;

            if (NavMesh.SamplePosition(desiredPosition, out NavMeshHit hit, checkRadius, NavMesh.AllAreas))
            {
                targetPosition = hit.position;
                agent.SetDestination(targetPosition);
                hasFleePoint = true;
                stateController.SetIsFleeing(true);

                predictedTimeToFleePoint = (Vector3.Distance(transform.position, targetPosition) / agent.speed) + TIME_TO_FLEE_POINT_TOLERANCE;
                Debug.Log(predictedTimeToFleePoint);

                Debug.DrawLine(transform.position, targetPosition, Color.green, 2f);
                Debug.DrawRay(targetPosition, Vector3.up * 2, Color.green, 2f); 

                return;
            }

            // Alternate angles: +angle, -angle, +2*angle, -2*angle...
            currentAngle = (i % 2 == 0 ? 1 : -1) * ((i + 1) / 2) * angleStep;
        }

        Debug.LogWarning("Failed to find a valid NavMesh point to flee to after multiple attempts.");
        hasFleePoint = false;
        stateController.SetIsFleeing(false);
        stateController.GoHome();
    }

    public IEnumerator StopMovement(float duration)
    {
        agent.speed = 0f;
        yield return new WaitForSeconds(duration);
        agent.speed = startSpeed; 
    }
}
