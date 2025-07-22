using UnityEngine;

public class FoxStateController : MonoBehaviour
{
    [SerializeField] private Transform homePoint;
    [SerializeField] private float maxDistanceFromHome = 20f;

    private FoxStateMachine stateMachine;
    private FoxAgent agent;
    private FoxAnimationController animationController;

    private float distanceCheckTimer;
    private float farFromHomeTimer;
    private bool isFarFromHome = false;
    private bool isFleeing = false;

    private const float IS_FAR_FROM_HOME_CHECK_RATE = 1f;
    private const float FAR_FROM_HOME_MAX_TIME = 10f;

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
        distanceCheckTimer += Time.deltaTime;

        if (distanceCheckTimer >= IS_FAR_FROM_HOME_CHECK_RATE)
        {
            isFarFromHome = Vector3.Distance(transform.position, homePoint.position) >= maxDistanceFromHome;

            distanceCheckTimer = 0f;
        }

        if (!isFleeing && isFarFromHome)
        {
            farFromHomeTimer += Time.deltaTime;

            if (farFromHomeTimer >= FAR_FROM_HOME_MAX_TIME)
            {
                GoHome();
                farFromHomeTimer = 0f;
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
        stateMachine.SetState(new FoxStateBackToHome(this, agent, animationController, homePoint.position));
    }

    public void TryFlee()
    {
        if (isFleeing) { return; }
        stateMachine.SetState(new FoxFleeState(agent, animationController));
    }

    public void SetIsFleeing(bool isFleeing)
    {
        this.isFleeing = isFleeing;
    }
}
