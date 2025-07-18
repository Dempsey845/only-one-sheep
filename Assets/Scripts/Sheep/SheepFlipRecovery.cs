using UnityEngine;
using System.Collections;

public class SheepFlipRecovery : MonoBehaviour
{
    [Header("Flip Detection")]
    public Transform feetDirectionTransform; // Set this to a child object that points down when upright
    public float upsideDownDotThreshold = 0f;
    public float checkDelay = 1f;

    [Header("Recovery Settings")]
    public float uprightTorque = 10f;
    public float uprightDuration = 2f;

    [Header("On Feet Detection")]
    [SerializeField] private float checkDistance;
    [SerializeField] private LayerMask checkMask;

    private Rigidbody rb;
    private float flippedTime = 0f;
    private bool isRecovering = false;

    public bool IsSheepOnFeet { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (feetDirectionTransform == null)
        {
            Debug.LogError("feetDirectionTransform is not set! Please assign a Transform that points down from the sheep.");
        }
    }

    void FixedUpdate()
    {
        // Use the direction the sheep’s “feet” point in
        Vector3 feetDirection = feetDirectionTransform.up; // should point down in local space
        float dot = Vector3.Dot(feetDirection, Vector3.up);

        IsSheepOnFeet = Physics.Raycast(feetDirectionTransform.position, feetDirection, checkDistance, checkMask);

        //Debug.Log($"[Flip Check] Feet-to-Up Dot: {dot:F2}");

        if (dot > upsideDownDotThreshold)
        {
            flippedTime += Time.fixedDeltaTime;
            //Debug.Log($"[Upside Down Detected] Time: {flippedTime:F2}s");

            if (flippedTime >= checkDelay && !isRecovering)
            {
                //Debug.Log("[Recovery Triggered] Starting recovery coroutine...");
                StartCoroutine(RotateUpright());
            }
        }
        else
        {
            if (flippedTime > 0f)
                //Debug.Log("[Flip Cancelled] Sheep is no longer upside down");

            flippedTime = 0f;
        }
    }

    private IEnumerator RotateUpright()
    {
        isRecovering = true;
        float timer = 0f;

        while (timer < uprightDuration)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            Quaternion deltaRotation = targetRotation * Quaternion.Inverse(rb.rotation);
            deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);

            //Debug.Log($"[Recovering] Angle: {angle:F1}, Axis: {axis}");

            if (angle > 0.1f && !float.IsNaN(axis.x))
            {
                rb.AddTorque(axis.normalized * uprightTorque, ForceMode.VelocityChange);
            }

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        //Debug.Log("[Recovery Complete] Sheep uprighted.");
        isRecovering = false;
    }
}
