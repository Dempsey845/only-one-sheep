using UnityEngine;

public class ExplosiveBarrel : Interactable
{
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float explosionForce = 20f;
    [SerializeField] private float upwardForce = 50f;
    [SerializeField] private LayerMask affectedLayers;
    [SerializeField] private int sheepDamage = 10;
    [SerializeField] private GameObject explosiveFXPrefab;

    protected override void Interact()
    {
        Explode();

        base.Interact();
    }

    private void Explode()
    {
        Vector3 explosionPosition = transform.position;

        Instantiate(explosiveFXPrefab, transform.position, Quaternion.identity);

        // Get all colliders in radius
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius, affectedLayers);

        if (sheepHealth != null)
        {
            SheepRagdollController ragdollController;
            sheepHealth.gameObject.TryGetComponent<SheepRagdollController>(out ragdollController);
            ragdollController.Collapse(6f);
        }

        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.attachedRigidbody;

            if (rb != null)
            {
                // Clear velocity for consistent launch
                rb.linearVelocity = Vector3.zero;

                // Calculate upward force manually
                Vector3 force = Vector3.up * upwardForce + (rb.position - explosionPosition).normalized * explosionForce;

                rb.AddForce(force, ForceMode.Impulse);
            }
        }

        sheepHealth.TakeDamage(sheepDamage);
    }
}
