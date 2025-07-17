using UnityEngine;
using UnityEngine.AI;

public class SheepWanderState : ISheepState
{
    private readonly SheepStateMachine sheep;
    private readonly SheepWander wander;

    private readonly float nextPointRate = 6f;
    private readonly float wanderRadius = 10f;

    public SheepWanderState(SheepStateMachine sheep, SheepWander wander)
    {
        this.sheep = sheep;
        this.wander = wander;
    }

    public void Enter()
    {
        wander.CanWander = true;
        wander.NextPointRate = nextPointRate;
        wander.WanderRadius = wanderRadius;
    }

    public void Update()
    {

    }

    public void Exit()
    {
        wander.CanWander = true;
    }
}
