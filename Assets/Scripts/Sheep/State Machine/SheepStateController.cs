using UnityEngine;

public class SheepStateController : MonoBehaviour
{
    public static SheepStateController Instance { get; private set; }
    public bool IsSheepCurious { get; private set; }
    public bool IsSheepPanicking { get; private set; }

    [SerializeField] private float startIdleDuration = 2f;

    private SheepStateMachine stateMachine;
    private Interactable interactableThatSheepIsInterestedIn;

    private void Awake()
    {
        Instance = this;
    }

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
        IsSheepPanicking = true;
        stateMachine.SetState(new SheepPanicState(stateMachine, GetComponent<SheepWander>(), GetComponent<SheepPhysicsNavAgent>(), this));
    }

    public void StopPanic()
    {
        IsSheepPanicking = false;
    }

    public void Curious(Vector3 interactablesPosition, Interactable interactable, float curiousDuration)
    {
        IsSheepCurious = true;
        interactableThatSheepIsInterestedIn = interactable;
        stateMachine.SetState(new SheepCuriousState(stateMachine, this, interactablesPosition, curiousDuration));
    }

    public void NotCuriousAnymore()
    {
        interactableThatSheepIsInterestedIn.IgnoreFromSheep = true;
        interactableThatSheepIsInterestedIn.IsSheepCuriousOfThis = false;
        interactableThatSheepIsInterestedIn = null;

        IsSheepCurious = false;
    }

    public void Idle(float idleDuration)
    {
        stateMachine.SetState(new SheepIdleState(stateMachine, idleDuration));
    }
}
