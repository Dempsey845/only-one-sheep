using UnityEngine;

public class FoxIdleState : IFoxState
{
    private const float DISTANCE_CHECK_RATE = 1f;
    private const float CHASE_RANGE = 10f;
    private const float CHASE_DURATION = 500f;
    private const float IDLE_DURATION = 2f;

    private readonly FoxStateController stateController;
    private readonly FoxAnimationController animationController;

    private float timer;
    private float idleTime;

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
        idleTime += Time.deltaTime;

        if (timer >= DISTANCE_CHECK_RATE)
        {
            if (stateController.IsSheepInRange(CHASE_RANGE))
            {
                stateController.Chase(CHASE_DURATION);
            }
            timer = 0f;
        }

        if (idleTime >= IDLE_DURATION)
        {
            idleTime = 0f;
            stateController.Wander();
        }
    }
}
