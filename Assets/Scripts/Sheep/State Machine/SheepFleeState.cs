using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class SheepFleeState : ISheepState
{
    private readonly Transform fleeFromTarget;
    private readonly Transform sheepTransform;
    private readonly SheepPhysicsNavAgent sheepPhysicsNavAgent;
    private readonly SheepStateController sheepStateController;

    private const float FLEE_CHECK_RADIUS = 20f;
    private const float FLEE_DISTANCE = 20f;
    private const float FLEE_RATE = 3f;
    private const float MOVE_SPEED_MULTIPLIER = 1f;

    private float timer = 0f;

    public SheepFleeState(Transform fleeFromTarget, Transform sheepTransform, SheepPhysicsNavAgent sheepPhysicsNavAgent, SheepStateController sheepStateController)
    {
        this.fleeFromTarget = fleeFromTarget;
        this.sheepTransform = sheepTransform;
        this.sheepPhysicsNavAgent = sheepPhysicsNavAgent;
        this.sheepStateController = sheepStateController;
    }

    public void Enter()
    {
        timer = FLEE_RATE;
        sheepPhysicsNavAgent.MoveSpeedMultiplier = MOVE_SPEED_MULTIPLIER;
    }

    public void Exit()
    {
        sheepPhysicsNavAgent.MoveSpeedMultiplier = 1f;
    }

    public void Update()
    {
        if (fleeFromTarget == null)
        {
            sheepStateController.Wander();
            return;
        }

        timer += Time.deltaTime;

        if (timer < FLEE_RATE)
        {
            return;
        }

        Flee();

        timer = 0f;
    }

    private void Flee()
    {
        Vector3 targetPosition = fleeFromTarget.position;
        Vector3 baseDirection = (sheepTransform.position - targetPosition).normalized;

        int maxAttempts = 5;
        float angleStep = 30f;
        float currentAngle = 0f;

        for (int i = 0; i < maxAttempts; i++)
        {
            // Rotate base direction slightly to find alternatives
            Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * baseDirection;
            Vector3 desiredPosition = sheepTransform.position + direction * FLEE_DISTANCE;

            if (NavMesh.SamplePosition(desiredPosition, out NavMeshHit hit, FLEE_CHECK_RADIUS, NavMesh.AllAreas))
            {
                targetPosition = hit.position;
                sheepPhysicsNavAgent.SetTargetPosition(targetPosition);

                Debug.DrawLine(sheepTransform.position, targetPosition, Color.green, 2f);
                Debug.DrawRay(targetPosition, Vector3.up * 2, Color.green, 2f);

                return;
            }

            // Alternate angles: +angle, -angle, +2*angle, -2*angle...
            currentAngle = (i % 2 == 0 ? 1 : -1) * ((i + 1) / 2) * angleStep;
        }
    }
}
