using System.Collections;
using UnityEngine;

public class SheepRagdollController : MonoBehaviour
{
    [Header("Ragdoll Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private Transform forwardDirectionTransform;

    [Header("Ragdoll Setup")]
    [SerializeField] private Rigidbody[] ragdollBodies;
    [SerializeField] private Transform rootBone;

    private Rigidbody rootBody;
    private SheepFlipRecovery recovery;

    private bool canMove = true;

    private Vector3 targetPosition;
    private Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;
        rootBody = GetComponent<Rigidbody>();
        recovery = GetComponent<SheepFlipRecovery>();
        StartCoroutine(RotationCheckRoutine());
    }

    private void FixedUpdate()
    {
        if (!canMove) { return; }

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
        }


        Vector3 forward = forwardDirectionTransform.forward;
        forward.y = 0f;

        Debug.DrawRay(forwardDirectionTransform.position, forwardDirectionTransform.forward * 2f, Color.green);

        Vector3 dirToTarget = targetPosition + Vector3.up - transform.position;
        dirToTarget.y = 0f;
        dirToTarget = dirToTarget.normalized;

        rootBody.linearVelocity = dirToTarget * moveSpeed;
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
        transform.rotation = startRotation;
        rootBody.freezeRotation = true;
        canMove = true;
    }

    public void SetTarget(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private IEnumerator RotationCheckRoutine()
    {
        const float checkInterval = 15f;
        const float angleThreshold = 100f;

        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (!canMove) continue;

            // Get the current and target rotation (only using X and Z axes for comparison)
            Quaternion current = transform.rotation;
            Quaternion target = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y, current.eulerAngles.z);

            // Calculate the angle difference (ignoring Y rotation)
            float angleDifference = Quaternion.Angle(current, target);

            if (angleDifference > angleThreshold)
            {
                Debug.Log($"[SheepRagdollController] Fixing rotation. Pitch/Roll off by {angleDifference:F2}°");

                // Only apply correction when there's a significant angle difference
                transform.rotation = target;
            }
        }
    }





}
