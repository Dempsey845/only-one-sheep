using UnityEngine;

public class SheepCuriousState : ISheepState
{
    private readonly SheepStateMachine sheep;
    private readonly SheepStateController stateController;
    private readonly Vector3 interactablePosition;

    private readonly float curiousDuration;

    private float timer;

    private SheepPhysicsNavAgent navAgent;

    public SheepCuriousState(SheepStateMachine sheep, SheepStateController stateController, Vector3 interactablePosition, float curiousDuration = 4f)
    {
        this.sheep = sheep;
        this.stateController = stateController;
        this.interactablePosition = interactablePosition;
        this.curiousDuration = curiousDuration;
    }

    public void Enter()
    {
        timer = 0f;

        navAgent = sheep.gameObject.GetComponent<SheepPhysicsNavAgent>();

        navAgent.SetTargetPosition(interactablePosition);
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= curiousDuration)
        {
            sheep.SetState(new SheepWanderState(sheep, sheep.GetComponent<SheepWander>()));
        }
    }

    public void Exit() 
    {
        stateController.NotCuriousAnymore();
    }
}
