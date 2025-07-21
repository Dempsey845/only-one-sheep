using UnityEngine;

public class FoxChaseState : IFoxState
{
    private readonly FoxStateController stateController;
    private readonly FoxAnimationController animationController;
    private readonly FoxAgent agent;
    private readonly float duration;

    private float timer;

    public FoxChaseState(FoxStateController stateController, FoxAnimationController animationController, FoxAgent agent, float duration)
    {
        this.stateController = stateController;
        this.animationController = animationController;
        this.agent = agent;
        this.duration = duration;
    }

    public void Enter()
    {
        agent.StartChasingSheep();
        animationController.PlayChase();
    }

    public void Exit()
    {
        agent.StopChasingSheep();
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer > duration)
        {
            stateController.Idle();
            timer = 0;
        }
    }
}
