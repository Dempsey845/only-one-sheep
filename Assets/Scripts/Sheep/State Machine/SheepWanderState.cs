using UnityEngine;
using UnityEngine.AI;

public class SheepWanderState : ISheepState
{
    private readonly SheepStateMachine sheep;
    private readonly SheepWander wander;

    private readonly float nextPointRate = 3f;
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

        SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Happy);
    }

    public void Update()
    {

    }

    public void Exit()
    {
        wander.CanWander = true;
    }
}
