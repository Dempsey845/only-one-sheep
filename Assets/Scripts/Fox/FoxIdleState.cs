using UnityEngine;

public class FoxIdleState : IFoxState
{
    private const float DISTANCE_CHECK_RATE = 1f;
    private const float CHASE_RANGE = 10f;
    private const float CHASE_DURATION = 5f;

    private readonly FoxStateController stateController;
    private readonly FoxAnimationController animationController;

    private float timer;

    public FoxIdleState(FoxStateController stateController, FoxAnimationController animationController)
    {
        this.stateController = stateController;
        this.animationController = animationController;
    }

    public void Enter()
    {
        animationController.PlayIdleAnimation();
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= DISTANCE_CHECK_RATE)
        {
            if (stateController.IsSheepInRange(CHASE_RANGE))
            {
                stateController.Chase(CHASE_DURATION);
            }
            timer = 0f;
        }
    }
}
