using UnityEngine;

public class SheepStateController : MonoBehaviour
{
    [SerializeField] private float startIdleDuration = 2f;
    private SheepStateMachine stateMachine;

    private void Start()
    {
        stateMachine = GetComponent<SheepStateMachine>();
        stateMachine.SetState(new SheepIdleState(stateMachine, startIdleDuration));
    }

    public void Wander()
    {
        stateMachine.SetState(new SheepWanderState(stateMachine, GetComponent<SheepWander>()));
    }

    public void ChasePlayer(float chaseDuration)
    {
        stateMachine.SetState(new SheepFollowState(stateMachine, chaseDuration));
    }

    public void Panic(float panicDuration)
    {
        stateMachine.SetState(new SheepPanicState(stateMachine, GetComponent<SheepWander>(), GetComponent<SheepPhysicsNavAgent>(), this));
    }
}
