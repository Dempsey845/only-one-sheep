using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsNavAgent : MonoBehaviour
{
    /* Rigidbody physics movement + Manual NavMesh pathing  */

    [Header("Navigation")]
    [SerializeField] private Transform target; // for testing purposes
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotateSpeed = 25f;
    [SerializeField] private float waypointTolerance = 0.2f;
    [SerializeField] private float repathRate = 1.0f;

    private Rigidbody rb;
    private NavMeshPath path;
    private int currentCornerIndex = 0;
    private float repathTimer;

    private Vector3 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        path = new NavMeshPath();
        CalculatePath();
    }

    private void Update()
    {
        // Calculate direction to the target
        Vector3 direction = (target.position - transform.position).normalized;

        // Ensure rotation is only on the Y-axis (yaw)
        direction.y = 0;

        // Rotate towards the target smoothly
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        repathTimer += Time.deltaTime;
        if (repathTimer >= repathRate)
        {
            CalculatePath();
            repathTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        FollowPath();
    }

    private void CalculatePath()
    {
        if (target != null && NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path))
        {
            currentCornerIndex = 0;
        }
    }

    private void FollowPath()
    {
        if (path == null || path.corners.Length == 0 || currentCornerIndex >= path.corners.Length)
            return;

        Vector3 targetCorner = path.corners[currentCornerIndex];
        direction = (targetCorner - transform.position).normalized;

        // Move the sheep towards the target
        Vector3 move = direction * moveSpeed;
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);

        // Advance to next waypoint if close
        if (Vector3.Distance(transform.position, targetCorner) <= waypointTolerance)
        {
            currentCornerIndex++;
        }
    }
}
