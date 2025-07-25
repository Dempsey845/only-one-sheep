using System.Collections;
using UnityEngine;

public class Sprinkler : Interactable
{
    [Header("Sprinkler")]
    [SerializeField] private GameObject burstFXPrefab;
    [SerializeField] private Transform burstFXSpawnPoint;
    [SerializeField] private Transform liftPoint;
    [SerializeField] private float cooldownDuration = 15f;
    [SerializeField] private float burstUpForce = 30f;
    [SerializeField] private float hoverThreshold = 0.2f; 

    private Rigidbody rootBody;
    private bool hasBurst = false;
    private bool isHovering = false;
    private float hoverDuration;

    private void FixedUpdate()
    {
        if (hasBurst && rootBody != null && !isHovering)
        {
            Vector3 direction = (liftPoint.position - rootBody.position).normalized;
            rootBody.linearVelocity = direction * burstUpForce;

            rootBody.position = new Vector3(liftPoint.position.x, rootBody.position.y, liftPoint.position.z);

            if (Vector3.Distance(rootBody.position, liftPoint.position) < hoverThreshold)
            {
                StartCoroutine(HoverAtLiftPoint());
            }
        }
    }

    protected override void Interact()
    {
        IsInteractable = false;

        ParticleSystem ps = Instantiate(burstFXPrefab, burstFXSpawnPoint).GetComponent<ParticleSystem>();
        float duration = ps.main.duration;

        Instantiate(interactSFXPrefab, transform.position, Quaternion.identity);

        SheepRagdollController ragdollController = sheepHealth.GetComponent<SheepRagdollController>();
        StartCoroutine(ragdollController.StopMovement(duration));
        StartCoroutine(ragdollController.StopRotationFix(duration));
        StartCoroutine(Cooldown(duration));
        ragdollController.ClearVelocity();
        rootBody = ragdollController.GetComponent<Rigidbody>();

        hoverDuration = duration - 1f;
        hasBurst = true;
    }

    private IEnumerator HoverAtLiftPoint()
    {
        isHovering = true;
        hasBurst = false;

        rootBody.useGravity = false;
        rootBody.linearVelocity = Vector3.zero;
        rootBody.position = liftPoint.position;
        rootBody.constraints = RigidbodyConstraints.FreezeAll;

        yield return new WaitForSeconds(hoverDuration);

        rootBody.constraints = RigidbodyConstraints.None;
        rootBody.useGravity = true;
        isHovering = false;

        Destroy(gameObject);
    }

    private IEnumerator Cooldown(float burstDuration)
    {
        yield return new WaitForSeconds(burstDuration + cooldownDuration);
        IsInteractable = true;
    }
}
