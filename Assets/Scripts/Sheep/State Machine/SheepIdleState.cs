using UnityEngine;

public class SheepIdleState : ISheepState
{
    private readonly SheepStateMachine sheep;
    private float idleTime;
    private float timer;

    public SheepIdleState(SheepStateMachine sheep, float idleDuration = 4f)
    {
        this.sheep = sheep;
        this.idleTime = idleDuration;
    }

    public void Enter()
    {
        timer = 0f;
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= idleTime)
        {
            sheep.SetState(new SheepWanderState(sheep, sheep.GetComponent<SheepWander>()));
        }
    }

    public void Exit() { }
}
