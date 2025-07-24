using System;
using UnityEngine;

public class ExplosiveBarrel : Interactable
{
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float explosionForce = 20f;
    [SerializeField] private float upwardForce = 50f;
    [SerializeField] private float collapseSheepTime = 10f;
    [SerializeField] private LayerMask affectedLayers;
    [SerializeField] private int sheepDamage = 10;
    [SerializeField] private GameObject explosiveFXPrefab;

    public event Action OnExplode;

    protected override void Interact()
    {
        Explode();

        base.Interact();
    }

    private void Explode()
    {
        bool playerInRange = (transform.position - PlayerManager.Instance.GetPosition()).sqrMagnitude <= explosionRadius * 2;
        Vector3 explosionPosition = transform.position;

        Instantiate(explosiveFXPrefab, transform.position, Quaternion.identity);

        // Get all colliders in radius
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius, affectedLayers);

        if (SheepManager.Instance != null)
        {
            SheepManager.Instance.RagdollController.Collapse(collapseSheepTime);

            SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Sad);
        }

        if (playerInRange)
        {
            TimeManager.Instance.DoSlowMotion(.2f, 2f);
            PlayerManager.Instance.PlayerMovement.StopMovement(1f);
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

        OnExplode?.Invoke();

        sheepHealth.TakeDamage(sheepDamage);
    }
}
