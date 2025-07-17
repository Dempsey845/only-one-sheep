using UnityEngine;
using UnityEngine.AI;

public class SheepWanderState : ISheepState
{
    private readonly SheepStateMachine sheep;
    private readonly SheepWander wander;

    public SheepWanderState(SheepStateMachine sheep, SheepWander wander)
    {
        this.sheep = sheep;
        this.wander = wander;
    }

    public void Enter()
    {
        wander.SetCanWander(true);
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        wander.SetCanWander(false);
    }
}
