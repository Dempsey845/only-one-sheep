using UnityEngine;

public class FoxDieState : IFoxState
{
    private readonly FoxAnimationController animationController;
    private readonly FoxAgent agent;

    public FoxDieState(FoxAnimationController animationController, FoxAgent agent)
    {
        this.animationController = animationController;
        this.agent = agent;
    }

    public void Enter()
    {
        agent.CancelDestination();
        animationController.PlayDeathAnimation();
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}
