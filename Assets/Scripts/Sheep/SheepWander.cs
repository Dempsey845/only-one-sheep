using UnityEngine;
using UnityEngine.AI;

public class SheepWander : MonoBehaviour
{
    [Tooltip("How often the sheep picks a new point to wander to")]
    [SerializeField] private float nextPointRate = 6.0f;

    [Tooltip("Maximum radius the sheep can wander from its current position")]
    [SerializeField] private float wanderRadius = 50f;

    private SheepPhysicsNavAgent sheepPhysicsNavAgent;
    private float timer = 0.0f;

    private bool canWander = false;

    private void Start()
    {
        sheepPhysicsNavAgent = GetComponent<SheepPhysicsNavAgent>();
        timer = nextPointRate;
    }

    private void Update()
    {
        if (!canWander) return;

        timer += Time.deltaTime;

        if (timer > nextPointRate)
        {
            Vector3 newDestination = GetRandomWanderPoint();
            sheepPhysicsNavAgent.SetTargetPosition(newDestination); 
            timer = 0.0f;
        }
    }

    private Vector3 GetRandomWanderPoint()
    {
        for (int i = 0; i < 30; i++) // Try up to 30 times
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            randomDirection.y = transform.position.y; // Keep height the same

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        // If nothing valid found, fallback to current position
        Debug.LogWarning("Sheep couldn't find valid Wander Point");
        return transform.position;
    }

    public void SetCanWander(bool canWander)
    {
        this.canWander = canWander;
    }
}
