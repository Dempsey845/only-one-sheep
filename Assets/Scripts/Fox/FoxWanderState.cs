using UnityEngine;
using UnityEngine.AI;

public class FoxWanderState : IFoxState
{
    private readonly FoxAgent foxAgent;
    private readonly Transform transform;
    private readonly FoxAnimationController animationController;
    private readonly FoxStateController stateController;

    private const float MIN_WANDER_DISTANCE = 5f;
    private const float REACHED_WANDER_POINT_TOLERANCE = 1f;
    private const float IDLE_AT_POINT_DURATION = 3f;
    private const float WANDER_RADIUS = 30f;
    private const float CALCULATE_DISTANCE_RATE = .5f;
    private const float TIME_TO_WANDER_POINT_TOLERANCE = 2f;
    private const float CHASE_DURATION = 20f;
    private const float CHASE_RANGE = 15f;
    private const float CHASE_CHECK_RATE = 1f;

    private Vector3 wanderPoint;
    private float atPointTimer = 0f;
    private float distanceTimer = 0f;
    private float distanceFromWanderPoint;
    private float predictedTimeToWanderPoint;
    private float wanderTimer = 0f;
    private float chaseTimer = 0f;

    public FoxWanderState(FoxAgent foxAgent, Transform transform, FoxAnimationController animationController, FoxStateController stateController)
    {
        this.foxAgent = foxAgent;
        this.transform = transform;
        this.animationController = animationController;
        this.stateController = stateController;
    }

    public void Enter()
    {
        NewWanderPoint();
        CalculateDistance();
        predictedTimeToWanderPoint = distanceFromWanderPoint / foxAgent.GetSpeed();
    }

    public void Exit()
    {
    }

    public void Update()
    {
        distanceTimer += Time.deltaTime;
        chaseTimer += Time.deltaTime;

        if (distanceTimer >= CALCULATE_DISTANCE_RATE)
        {
            CalculateDistance();
            distanceTimer = 0f;
        }

        if (distanceFromWanderPoint < REACHED_WANDER_POINT_TOLERANCE)
        {
            if (atPointTimer == 0f)
            {
                animationController.PlayIdleAnimation();
            }

            atPointTimer += Time.deltaTime;

            if (atPointTimer >= IDLE_AT_POINT_DURATION)
            {
                HandleReachedWanderPoint();
            }
        }
        else
        {
            wanderTimer += Time.deltaTime;

            if (wanderTimer >= predictedTimeToWanderPoint + TIME_TO_WANDER_POINT_TOLERANCE)
            {
                wanderTimer = 0f;
                HandleReachedWanderPoint();
            }
        }

        if (chaseTimer > CHASE_CHECK_RATE)
        {
            chaseTimer = 0f;
            ChaseCheck();
        }
    }

    private void ChaseCheck()
    {
        if (stateController.IsSheepInRange(CHASE_RANGE))
        {
            stateController.Chase(CHASE_DURATION);
        }
    }

    private void HandleReachedWanderPoint()
    {
        NewWanderPoint();
        atPointTimer = 0f;
        CalculateDistance();
        predictedTimeToWanderPoint = distanceFromWanderPoint / foxAgent.GetSpeed();
    }

    private void CalculateDistance()
    {
        distanceFromWanderPoint = Vector3.Distance(transform.position, wanderPoint);
    }

    private void NewWanderPoint()
    {
        wanderPoint = GetRandomWanderPoint();
        foxAgent.GoToDestination(wanderPoint);
        animationController.PlayRunAnimation();
        wanderTimer = 0f;
    }

    private Vector3 GetRandomWanderPoint()
    {
        const int MAX_ATTEMPTS = 30;

        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * WANDER_RADIUS;
            randomDirection += transform.position;
            randomDirection.y = transform.position.y;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                if (Vector3.Distance(transform.position, hit.position) >= MIN_WANDER_DISTANCE)
                {
                    return hit.position;
                }
            }
        }

        return transform.position;
    }
}