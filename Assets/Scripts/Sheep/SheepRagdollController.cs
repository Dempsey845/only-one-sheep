using UnityEngine;
using System.Collections.Generic;

public class SheepRagdollController : MonoBehaviour
{
    [Header("Ragdoll Setup")]
    public Rigidbody[] ragdollBodies;      // All ragdoll rigidbodies
    public Rigidbody parentRigidbody;      // Parent's Rigidbody
    public Collider parentCollider;        // Parent's Collider
    public Transform rootBone;             // Usually "Hips" or "Pelvis"

    [Header("Ragdoll Blend Settings")]
    public float blendDuration = 1f;

    private float blendTimer = 0f;
    private bool isBlending = false;

    private bool isRagdoll = true;

    private Dictionary<Transform, Vector3> blendStartPositions = new();
    private Dictionary<Transform, Quaternion> blendStartRotations = new();

    // Cache of initial local positions and rotations
    private Dictionary<Transform, Vector3> boneLocalPositions = new();
    private Dictionary<Transform, Quaternion> boneLocalRotations = new();

    private Quaternion rootBoneStartRotation;

    void Start()
    {
        rootBoneStartRotation = rootBone.rotation;

        // Store initial local transforms for reset
        foreach (var rb in ragdollBodies)
        {
            Transform t = rb.transform;
            boneLocalPositions[t] = t.localPosition;
            boneLocalRotations[t] = t.localRotation;
        }

        SetRagdoll(true); // Start in ragdoll mode
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isRagdoll = !isRagdoll;
            SetRagdoll(isRagdoll);
        }

        if (isBlending)
        {
            blendTimer += Time.deltaTime;
            float t = Mathf.Clamp01(blendTimer / blendDuration);

            foreach (var rb in ragdollBodies)
            {
                Transform bone = rb.transform;

                if (blendStartPositions.TryGetValue(bone, out Vector3 startPos) &&
                    boneLocalPositions.TryGetValue(bone, out Vector3 targetPos))
                {
                    bone.localPosition = Vector3.Lerp(startPos, targetPos, t);
                }

                if (blendStartRotations.TryGetValue(bone, out Quaternion startRot) &&
                    boneLocalRotations.TryGetValue(bone, out Quaternion targetRot))
                {
                    bone.localRotation = Quaternion.Slerp(startRot, targetRot, t);
                }
            }

            if (t >= 1f)
            {
                isBlending = false;
            }
        }
    }

    void SetRagdoll(bool state)
    {
        // Toggle ragdoll rigidbodies
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = !state;
            rb.detectCollisions = state;

            if (state)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        // Toggle parent Rigidbody and Collider
        if (parentRigidbody != null)
        {
            parentRigidbody.isKinematic = state;
            parentRigidbody.linearVelocity = Vector3.zero;
            parentRigidbody.angularVelocity = Vector3.zero;
        }

        if (parentCollider != null)
        {
            parentCollider.enabled = !state;
        }

        // On disable, align parent and start blend
        if (!state && rootBone != null)
        {
            transform.position = rootBone.position + Vector3.up;
            transform.rotation = Quaternion.identity;

            // Capture current bone transforms to blend from
            blendStartPositions.Clear();
            blendStartRotations.Clear();

            foreach (var rb in ragdollBodies)
            {
                Transform t = rb.transform;
                blendStartPositions[t] = t.localPosition;
                blendStartRotations[t] = t.localRotation;
            }

            blendTimer = 0f;
            isBlending = true;

            // Reset rootBone itself immediately
            rootBone.localPosition = Vector3.zero;
            rootBone.localRotation = rootBoneStartRotation;
        }
    }
}
