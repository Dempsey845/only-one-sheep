using UnityEngine;
using System.Collections;

public class SheepRagdollRecovery : MonoBehaviour
{
    public Rigidbody[] ragdollBodies;
    public float checkInterval = 0.5f;
    public float recoveryDelay = 2f;
    public float standUpThreshold = 0.5f;
    public float uprightSpeed = 5f;

    private bool isRagdolled = false;
    private bool isRecovering = false;

    void Start()
    {
        SetRagdollState(true);
        StartCoroutine(CheckFlippedRoutine());
    }

    private void LateUpdate()
    {
        if (isRagdolled)
        {
            FollowHips();
        }
    }

    void FollowHips()
    {
        Transform hips = ragdollBodies[0].transform; // Ensure this is the actual hips

        // Sync parent position to hips
        transform.position = hips.position;

        // Sync rotation (Y-axis only to avoid flipping)
        Vector3 flatForward = Vector3.ProjectOnPlane(hips.forward, Vector3.up).normalized;
        if (flatForward.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(flatForward, Vector3.up);
        }
    }


    void SetRagdollState(bool state)
    {
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = !state;
            rb.detectCollisions = state;
        }

        isRagdolled = state;
    }

    IEnumerator CheckFlippedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (!isRagdolled) continue;

            // Use hips or core body part
            Vector3 up = ragdollBodies[0].transform.up;
            if (!isRecovering && Vector3.Dot(up, Vector3.up) < standUpThreshold)
            {
                isRecovering = true;
                yield return new WaitForSeconds(recoveryDelay);
                StartCoroutine(StandUp());
            }
        }
    }

    IEnumerator StandUp()
    {
        // Freeze ragdoll
        SetRagdollState(false);

        // Snap position to hips
        Transform hips = ragdollBodies[0].transform;
        transform.position = hips.position;

        // Smoothly rotate upright
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(hips.forward, Vector3.up), Vector3.up);

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed);
            elapsed += Time.deltaTime * uprightSpeed;
            yield return null;
        }

        transform.rotation = targetRot;

        // Optionally re-enable movement (e.g., nav agent)
        var nav = GetComponent<SheepPhysicsNavAgent>();
        if (nav != null)
        {
            nav.enabled = true;
        }

        isRecovering = false;
    }

    public void GoRagdoll()
    {
        SetRagdollState(true);
        isRecovering = false;

        var nav = GetComponent<SheepPhysicsNavAgent>();
        if (nav != null)
        {
            nav.enabled = false;
        }
    }
}
