using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SheepRagdollController : MonoBehaviour
{
    [Header("Ragdoll Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotateSpeed = 90f;
    [SerializeField] private Transform forwardDirectionTransform;

    [Header("Ragdoll Fix Rotation")]
    [SerializeField] private Transform feetDirection;
    [SerializeField] private float feetRaycastDistance = 2f;
    [SerializeField] private LayerMask uprightCheckMask;
    [SerializeField] private float fixRotateSpeed = 180f;

    [Header("Ragdoll Setup")]
    [SerializeField] private Rigidbody[] ragdollBodies;
    [SerializeField] private Transform rootBone;

    private Rigidbody rootBody;
    private SheepPhysicsNavAgent physicsNavAgent;

    private bool canMove = true;
    private bool fixingRotation = false;
    private bool fixRotation = true;
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

        if (fixRotation && fixingRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotation, fixRotateSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, startRotation) < 1f)
            {
                Debug.Log("Fixed rotation");
                fixingRotation = false;
            }

            return;
        }

        if (!canMove) { return; }

        if (!physicsNavAgent.HasTarget()) { return; }

        Vector3 toTarget = targetPosition - transform.position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude > 0.01f)
        {
            Quaternion currentRot = rootBody.rotation;

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

            Debug.DrawRay(forwardDirectionTransform.position, sheepForward * 2f, Color.green);

            rootBody.linearVelocity = flatToTarget * moveSpeed * physicsNavAgent.MoveSpeedMultiplier;
        }
    }


    private void OnDrawGizmos()
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

    private IEnumerator RotationCheckRoutine()
    {
        const float checkInterval = 0.5f;
        const float angleThreshold = 100f;

        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (fixingRotation || !fixRotation) continue;

            bool flowControl = FixRotation(angleThreshold);
            if (!flowControl)
            {
                continue;
            }
        }
    }

    private bool FixRotation(float angleThreshold)
    {
        if (feetDirection == null || !fixRotation) return false;

        // Raycast downwards from the feet to check if the sheep is upright
        Vector3 origin = feetDirection.position;
        Vector3 direction = -feetDirection.up;

        Debug.DrawRay(origin, direction * feetRaycastDistance, Color.red, 1.5f);

        if (!Physics.Raycast(origin, direction, feetRaycastDistance, uprightCheckMask))
        {
            StartCoroutine(StopMovement(2f));

            Debug.Log("Fixing rotation");
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
        fixRotation = false;
        rootBody.freezeRotation = false;
        isBeingDragged = true;
        yield return new WaitForSeconds(duration);
        rootBody.freezeRotation = true;
        fixRotation = true;
        isBeingDragged = false;
    }

    public IEnumerator StopRotationFix(float duration)
    {
        fixRotation = false;
        yield return new WaitForSeconds(duration);
        fixRotation = true;
    }
}
