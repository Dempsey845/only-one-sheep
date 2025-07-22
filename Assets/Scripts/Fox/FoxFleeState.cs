using UnityEngine;

public class FoxFleeState : IFoxState
{
    private readonly FoxAgent agent;
    private readonly FoxAnimationController foxAnimationController;

    public FoxFleeState(FoxAgent agent, FoxAnimationController foxAnimationController)
    {
        this.agent = agent;
        this.foxAnimationController = foxAnimationController;
    }

    public void Enter()
    {
        agent.TryFlee();
        foxAnimationController.PlayRunAnimation();
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }
}
