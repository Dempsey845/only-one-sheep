using UnityEngine;
using UnityEngine.AI;

public class SheepPanicState : ISheepState
{
    private readonly SheepStateMachine sheep;
    private readonly SheepWander wander;
    private readonly SheepPhysicsNavAgent navAgent;
    private readonly SheepStateController stateController;

    private readonly float moveSpeedMultiplier;
    private readonly float nextPointRate = 1.5f;
    private readonly float wanderRadius = 15f;

    private float panicDuration;
    private float timer;

    public SheepPanicState(
        SheepStateMachine sheep, 
        SheepWander wander, 
        SheepPhysicsNavAgent navAgent,
        SheepStateController stateController,
        float moveSpeedMultiplier = 2f, 
        float panicDuration = 10f)
    {
        this.sheep = sheep;
        this.wander = wander;
        this.navAgent = navAgent;
        this.stateController = stateController;
        this.moveSpeedMultiplier = moveSpeedMultiplier;
        this.panicDuration = panicDuration;
    }

    public void Enter()
    {
        wander.CanWander = true;
        wander.NextPointRate = nextPointRate;
        wander.WanderRadius = wanderRadius;
        navAgent.MoveSpeedMultiplier = moveSpeedMultiplier;

        timer = 0f;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer > panicDuration)
        {
            stateController.Wander();
        }
    }

    public void Exit()
    {
        wander.CanWander = false;
        navAgent.MoveSpeedMultiplier = 1f;
        SheepStateController.Instance.StopPanic();
    }
}
