using UnityEngine;

public class FoxStateBackToHome : IFoxState
{
    private readonly FoxStateController stateController;
    private readonly FoxAgent agent;
    private readonly Vector3 homePosition;

    private const float HAS_REACHED_HOME_CHECK_RATE = 1f;

    private float timer = 0f;

    public FoxStateBackToHome(FoxStateController stateController, FoxAgent agent, Vector3 homePosition)
    {
        this.stateController = stateController;
        this.agent = agent;
        this.homePosition = homePosition;
    }

    public void Enter()
    {
        agent.GoToDestination(homePosition);
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= HAS_REACHED_HOME_CHECK_RATE)
        {
            if (agent.HasReachedDestination(homePosition))
            {
                stateController.Idle();
            }

            timer = 0f;
        }
    }
}
