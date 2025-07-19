using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class SheepPhysicsNavAgent : MonoBehaviour
{
    /* Rigidbody physics movement + Manual NavMesh pathing  */

    [Header("Navigation")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotateSpeed = 25f;
    [SerializeField] private float waypointTolerance = 0.2f;
    [SerializeField] private float targetTolerance = 0.2f;
    [SerializeField] private float repathRate = 1.0f;

    private Rigidbody rb;
    private NavMeshPath path;
    private int currentCornerIndex = 0;
    private float repathTimer;

    private Vector3 direction;

    private Vector3 targetPosition;
    private bool hasTarget = false;

    public float MoveSpeedMultiplier { get; set; } = 1.0f;

    private bool isRagdollSheep;
    private SheepRagdollController sheepRagdollController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        path = new NavMeshPath();
        CalculatePath();
    }

    private void Start()
    {
        isRagdollSheep = TryGetComponent<SheepRagdollController>(out sheepRagdollController);
    }

    private void Update()
    {
        if (!hasTarget) return;

        if (!isRagdollSheep && !HasReachedTargetPosition() && path != null && path.corners.Length > 0 && currentCornerIndex < path.corners.Length)
        {
            RotateTowardsTargetPosition(path.corners[currentCornerIndex]); 
        }

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
        direction = (targetCorner - transform.position).normalized;

        if (isRagdollSheep)
        {
            sheepRagdollController.SetTarget(targetCorner);
        } else
        {
            // Smoothly move the sheep towards the target corner instead of jumping directly
            MoveTowardsTarget(targetCorner);
        }

        // Advance to the next waypoint if close enough
        if (Vector3.Distance(transform.position, targetCorner) <= waypointTolerance)
        {
            currentCornerIndex++;
        }
    }

    private void MoveTowardsTarget(Vector3 target)
    {
        // Smooth movement to prevent sudden jumps
        Vector3 smoothMove = Vector3.MoveTowards(transform.position, target, moveSpeed * MoveSpeedMultiplier * Time.fixedDeltaTime);
        rb.MovePosition(smoothMove);
    }

    private void RotateTowardsTargetPosition(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion newRotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        rb.MoveRotation(newRotation); 
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
