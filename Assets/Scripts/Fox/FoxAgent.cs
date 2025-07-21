using UnityEngine;
using UnityEngine.AI;

public class FoxAgent : MonoBehaviour
{
    [SerializeField] private float targetCheckRate = 1f;
    [SerializeField] private float stopDistance = 2f;

    private Vector3 targetPosition;
    private NavMeshAgent agent;

    private float timer = 0f;
    private bool chaseSheep = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        StopChasingSheep();
    }

    private void Update()
    {
        if (SheepManager.Instance == null) { return; }

        if (chaseSheep)
        {
            ChaseSheep();
        }
    }

    private void ChaseSheep()
    {
        timer += Time.deltaTime;

        if (timer > targetCheckRate)
        {
            Vector3 targetPosition = SheepManager.Instance.transform.position;
            float distance = Vector3.Distance(transform.position, targetPosition);

            if (distance <= stopDistance)
            {
                agent.SetDestination(transform.position);
            }
            else
            {
                agent.SetDestination(targetPosition);
            }

            timer = 0f;
        }
    }

    public void StopChasingSheep()
    {
        chaseSheep = false; ;
        agent.SetDestination(transform.position);
    }

    public void StartChasingSheep()
    {
        chaseSheep = true;
    }

    public void GoToDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public bool HasReachedDestination(Vector3 destination)
    {
        float distance = Vector3.Distance(transform.position, destination);

        return distance <= stopDistance;
    }
}
