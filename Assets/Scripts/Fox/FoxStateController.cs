using UnityEngine;

public class FoxStateController : MonoBehaviour
{
    [SerializeField] private Transform homePoint;
    [SerializeField] private float maxDistanceFromHome = 20f;

    private FoxStateMachine stateMachine;
    private FoxAgent agent;
    private FoxAnimationController animationController;

    private float checkTimer;
    private float toFarFromHomeTimer;
    private bool isToFarFromHome = false;

    private const float IS_TOO_FAR_FROM_HOME_CHECK_RATE = 1f;
    private const float TOO_FAR_FROM_HOME_MAX_TIME = 10f;

    private void Awake()
    {
        stateMachine = GetComponent<FoxStateMachine>();
        agent = GetComponent<FoxAgent>();
        animationController = GetComponent<FoxAnimationController>();
    }

    private void Start()
    {
        Idle();
    }

    private void Update()
    {
        checkTimer += Time.deltaTime;

        if (checkTimer >= IS_TOO_FAR_FROM_HOME_CHECK_RATE)
        {
            isToFarFromHome = Vector3.Distance(transform.position, homePoint.position) >= maxDistanceFromHome;

            checkTimer = 0f;
        }

        if (isToFarFromHome)
        {
            toFarFromHomeTimer += Time.deltaTime;

            if (toFarFromHomeTimer >= TOO_FAR_FROM_HOME_MAX_TIME)
            {
                GoHome();
                toFarFromHomeTimer = 0f;
            }
        }
    }

    public void Idle()
    {
        stateMachine.SetState(new FoxIdleState(this, animationController));
    }

    public void Chase(float chaseDuration)
    {
        stateMachine.SetState(new FoxChaseState(this, animationController, agent, chaseDuration));
    }

    public bool IsSheepInRange(float range)
    {
        if (SheepManager.Instance == null) { return false; }

        return Vector3.Distance(transform.position, SheepManager.Instance.transform.position) <= range;
    }

    public void GoHome()
    {
        stateMachine.SetState(new FoxStateBackToHome(this, agent, homePoint.position));
    }
}
