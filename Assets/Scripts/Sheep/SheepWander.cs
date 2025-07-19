using UnityEngine;
using UnityEngine.AI;

public class SheepWander : MonoBehaviour
{
    [SerializeField] private float minWanderDistance = 2f;

    private SheepPhysicsNavAgent sheepPhysicsNavAgent;
    private float timer = 0.0f;

    public bool CanWander { get; set; } = false;
    public float NextPointRate { get; set; } = 6.0f;
    public float WanderRadius { get; set; } = 50f;

    private void Start()
    {
        sheepPhysicsNavAgent = GetComponent<SheepPhysicsNavAgent>();
        timer = NextPointRate;
    }

    private void Update()
    {
        if (!CanWander) return;

        timer += Time.deltaTime;

        if (timer > NextPointRate)
        {
            Vector3 newDestination = GetRandomWanderPoint();
            sheepPhysicsNavAgent.SetTargetPosition(newDestination); 
            timer = 0.0f;
        } else
        {
            if (sheepPhysicsNavAgent.HasReachedTargetPosition())
            {
                sheepPhysicsNavAgent.RemoveTarget();
            }
        }
    }

    private Vector3 GetRandomWanderPoint()
    {
        for (int i = 0; i < 30; i++) // Try up to 30 times
        {
            Vector3 randomDirection = Random.insideUnitSphere * WanderRadius;
            randomDirection += transform.position;
            randomDirection.y = transform.position.y; // Keep height the same

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                if (Vector3.Distance(transform.position, hit.position) >= minWanderDistance)
                {
                    return hit.position;
                }
            }
        }

        // If nothing valid found, fallback to current position
        Debug.LogWarning("Sheep couldn't find valid Wander Point");
        return transform.position;
    }
}
