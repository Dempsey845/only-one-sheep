using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SheepRagdollController : MonoBehaviour
{
    [Header("Ragdoll Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotateSpeed = 90f;
    [SerializeField] private Transform forwardDirectionTransform;
    [SerializeField] private float stopDistance = 2f;

    [Header("Ragdoll Fix Rotation")]
    [SerializeField] private Transform feetDirection;
    [SerializeField] private float feetRaycastDistance = 2f;
    [SerializeField] private LayerMask uprightCheckMask;
    [SerializeField] private float fixRotateSpeed = 180f;
    [SerializeField] private float fixAngleThreshold = 200f;

    [Header("Ragdoll Setup")]
    [SerializeField] private Rigidbody[] ragdollBodies;
    [SerializeField] private Transform rootBone;

    private Rigidbody rootBody;
    private SheepPhysicsNavAgent physicsNavAgent;

    private bool canMove = true;
    private bool fixingRotation = false;
    private bool canFixRotation = true;
    private bool isBeingDragged = false;

    private Vector3 targetPosition;
    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;
        rootBody = GetComponent<Rigidbody>();
        physicsNavAgent = GetComponent<SheepPhysicsNavAgent>();
        StartCoroutine(RotationCheckRoutine());
    }

    private void FixedUpdate()
    {
        if (isBeingDragged) { return; }

        if (canFixRotation && fixingRotation)
        {
            FixRotation();

            return;
        }

        if (!canMove || !physicsNavAgent.HasTarget()) { return; }

        Vector3 toTarget = targetPosition - transform.position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude > 0.01f)
        {
            MoveAndRotateTowardsTarget(toTarget);
        }
    }

    private void FixRotation()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotation, fixRotateSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, startRotation) < 1f)
        {
            fixingRotation = false;
        }
    }

    private void MoveAndRotateTowardsTarget(Vector3 toTarget)
    {
        Quaternion currentRot = rootBody.rotation;

        // Stop moving if close enough
        if (toTarget.sqrMagnitude < stopDistance * 2)
        {
            rootBody.linearVelocity = Vector3.zero;
            return;
        }

        // The sheep's "real" forward axis
        Vector3 sheepForward = transform.up;

        // Flatten toTarget to horizontal
        Vector3 flatToTarget = toTarget.normalized;

        // Build rotation that turns sheepForward to face the target
        Quaternion targetRot = Quaternion.FromToRotation(sheepForward, flatToTarget) * currentRot;

        // Ensure quaternion stays normalized
        targetRot = Quaternion.Normalize(targetRot);

        // Rotate with physics
        rootBody.MoveRotation(Quaternion.RotateTowards(currentRot, targetRot, rotateSpeed * Time.fixedDeltaTime));

        rootBody.linearVelocity = flatToTarget * moveSpeed * physicsNavAgent.MoveSpeedMultiplier;
    }

    public void Collapse(float duration)
    {
        rootBody.freezeRotation = false;
        canMove = false;
        rootBody.linearVelocity = Vector3.zero;
        StartCoroutine(CollapseTime(duration));
    }

    public IEnumerator CollapseTime(float duration)
    {
        yield return new WaitForSeconds(duration);

        fixingRotation = true;
        StartCoroutine(StopMovement(5f));
    }

    public void SetTarget(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    public IEnumerator Gravitate(float duration)
    {
        canMove = false;
        rootBody.freezeRotation = false;
        canFixRotation = false;

        rootBody.linearVelocity = Vector3.zero;

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.useGravity = false;
        }

        rootBody.angularVelocity = Vector3.right * 50f;

        yield return new WaitForSeconds(duration);

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.useGravity = true;
        }

        canMove = true;
        rootBody.freezeRotation = true;
        canFixRotation = true;
    }

    public void GravitateForDuration(float duration)
    {
        StartCoroutine(Gravitate(duration));
    }

    private IEnumerator RotationCheckRoutine()
    {
        const float checkInterval = 0.5f;

        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (fixingRotation || !canFixRotation) continue;

            bool flowControl = IsSheepUpright(fixAngleThreshold);
            if (!flowControl)
            {
                continue;
            }
        }
    }

    private bool IsSheepUpright(float angleThreshold)
    {
        if (feetDirection == null || !canFixRotation) return false;

        // Raycast downwards from the feet to check if the sheep is upright
        Vector3 origin = feetDirection.position;
        Vector3 direction = -feetDirection.up;

        Debug.DrawRay(origin, direction * feetRaycastDistance, Color.red, 1.5f);

        if (!Physics.Raycast(origin, direction, feetRaycastDistance, uprightCheckMask))
        {
            StartCoroutine(StopMovement(0.5f));

            fixingRotation = true;
            
            return true;
        }

        // If raycast hit something, sheep is likely upright — do nothing
        return false;
    }

    public IEnumerator StopMovement(float duration)
    {
        canMove = false;
        rootBody.freezeRotation = false;
        yield return new WaitForSeconds(duration);
        canMove = true;
        rootBody.freezeRotation = true;
    }

    public IEnumerator StartDrag(float duration)
    {
        canFixRotation = false;
        rootBody.freezeRotation = false;
        isBeingDragged = true;
        yield return new WaitForSeconds(duration);
        rootBody.freezeRotation = true;
        canFixRotation = true;
        isBeingDragged = false;
    }

    public IEnumerator StopRotationFix(float duration)
    {
        canFixRotation = false;
        yield return new WaitForSeconds(duration);
        canFixRotation = true;
    }

    public void ClearVelocity()
    {
        rootBody.linearVelocity = Vector3.zero;
    }

    private void OnDrawGizmosSelected()
    {
        if (forwardDirectionTransform != null)
        {
            // Draw forward movement direction
            Gizmos.color = Color.green;
            Gizmos.DrawRay(forwardDirectionTransform.position, forwardDirectionTransform.forward * 2f);
            Gizmos.DrawSphere(forwardDirectionTransform.position + forwardDirectionTransform.forward * 2f, 0.05f);
        }

        // Draw line to target
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, targetPosition);
        Gizmos.DrawSphere(targetPosition, 0.1f);

    }
}
