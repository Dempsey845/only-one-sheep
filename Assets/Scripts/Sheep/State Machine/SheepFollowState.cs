using UnityEngine;

public class SheepFollowState : ISheepState
{
    private readonly SheepStateMachine sheep;
    private float followDuration;
    private float timer;

    private readonly float positionCheckRate = 0.2f;
    private float positionCheckTimer;

    private SheepPhysicsNavAgent navAgent;

    public SheepFollowState(SheepStateMachine sheep, float followDuration = 4f)
    {
        this.sheep = sheep;
        this.followDuration = followDuration;
    }

    public void Enter()
    {
        timer = 0f;
        positionCheckTimer = 0f;

        navAgent = sheep.gameObject.GetComponent<SheepPhysicsNavAgent>();
    }

    public void Update()
    {
        timer += Time.deltaTime;
        positionCheckTimer += Time.deltaTime;

        if (timer >= followDuration)
        {
            sheep.SetState(new SheepWanderState(sheep, sheep.GetComponent<SheepWander>()));
        }

        if (positionCheckTimer > positionCheckRate)
        {
            navAgent.SetTargetPosition(PlayerManager.Instance.GetPosition(), true);
            positionCheckTimer = 0f;
        }
    }

    public void Exit() { }
}
