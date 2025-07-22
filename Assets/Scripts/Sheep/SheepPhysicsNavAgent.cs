using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class SheepPhysicsNavAgent : MonoBehaviour
{
    /* Manual NavMesh pathing for Rigidbody physics movement  */

    [Header("Navigation")]
    [SerializeField] private float waypointTolerance = 0.2f;
    [SerializeField] private float targetTolerance = 0.2f;
    [SerializeField] private float repathRate = 1.0f;

    private Rigidbody rb;
    private NavMeshPath path;
    private int currentCornerIndex = 0;
    private float repathTimer;

    private Vector3 targetPosition;
    private bool hasTarget = false;

    public float MoveSpeedMultiplier { get; set; } = 1.0f;

    private SheepRagdollController sheepRagdollController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        path = new NavMeshPath();
        CalculatePath();
    }

    private void Start()
    {
        sheepRagdollController = GetComponent<SheepRagdollController>();
    }

    private void Update()
    {
        if (!hasTarget) return;

        repathTimer += Time.deltaTime;
        if (repathTimer >= repathRate)
        {
            // Only recalculate the path if the target position has moved significantly
            if (Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                CalculatePath();
                repathTimer = 0f;
            }
        }
    }

    private void CalculatePath()
    {
        if (!hasTarget) return;

        // Recalculate the path from the sheep's current position to the target position
        if (NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path))
        {
            currentCornerIndex = 0;
        }
        else
        {
            Debug.LogWarning("Failed to calculate path to target: " + targetPosition);
        }
    }

    private void FixedUpdate()
    {
        FollowPath();
    }

    private void FollowPath()
    {
        if (path == null || path.corners.Length == 0 || currentCornerIndex >= path.corners.Length)
            return;

        Vector3 targetCorner = path.corners[currentCornerIndex];

        sheepRagdollController.SetTarget(targetCorner);

        // Advance to the next waypoint if close enough
        if (Vector3.Distance(transform.position, targetCorner) <= waypointTolerance)
        {
            currentCornerIndex++;
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        // Snap the target position to the nearest point on the NavMesh
        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            this.targetPosition = hit.position;
            hasTarget = true;
            CalculatePath();
            repathTimer = 0f;
        }
        else
        {
            Debug.LogWarning("Target position is not on the NavMesh: " + targetPosition);
            hasTarget = false;
        }
    }

    public void RemoveTarget()
    {
        this.targetPosition = Vector3.zero;
        hasTarget = false;
    }

    public bool HasReachedTargetPosition()
    {
        return Vector3.Distance(transform.position, targetPosition) <= targetTolerance;
    }

    public bool HasTarget() { return hasTarget; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 0.2f);

        if (path != null && path.corners.Length > 1)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
            }
        }
    }
}
