using System.Collections;
using UnityEngine;

public class SheepDrag : MonoBehaviour
{
    [SerializeField] private float dragSpeed = 5f;
    [SerializeField] private float stopDistance = 1.5f;

    private SheepRagdollController ragdollController;
    private Rigidbody rb;

    private bool isDragging = false;

    private void Awake()
    {
        ragdollController = GetComponent<SheepRagdollController>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (PlayerManager.Instance == null)
        {
            StopDrag();
            return;
        }

        if (isDragging)
        {
            Vector3 playerPosition = PlayerManager.Instance.GetPosition();
            Vector3 direction = (playerPosition - transform.position);
            direction.y = 0f;

            float distance = direction.magnitude;

            if (distance > stopDistance)
            {
                Vector3 velocity = direction.normalized * dragSpeed;
                rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
            }
            else
            {
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }
        }
    }

    public void StopDrag()
    {
        isDragging = false;
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        SheepStateController.Instance.Wander();
    }

    public void StartDragging(float duration)
    {
        isDragging = true;
        StartCoroutine(ragdollController.StopMovement(duration));
        StartCoroutine(ragdollController.StopRotationFix(duration));
        StartCoroutine(DragTime(duration));
    }

    private IEnumerator DragTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        StopDrag();
    }
}
