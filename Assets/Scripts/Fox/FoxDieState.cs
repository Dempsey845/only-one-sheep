using UnityEngine;

public class FoxDieState : IFoxState
{
    private readonly FoxAnimationController animationController;

    public FoxDieState(FoxAnimationController animationController, FoxAgent agent)
    {
        this.animationController = animationController;
    }

    public void Enter()
    {
        animationController.PlayDeathAnimation();
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}
