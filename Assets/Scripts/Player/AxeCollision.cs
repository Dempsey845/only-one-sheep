using System.Collections;
using UnityEngine;

public class AxeCollision : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject axeHitFxPrefab;
    [SerializeField] private GameObject foxHitFxPrefab;

    private bool canBeHit = true;

    private const float HIT_RATE = 0.3f;

    private void OnTriggerEnter(Collider other)
    {
        if (!canBeHit) { return; }

        if (other.CompareTag("Fox"))
        {
            FoxStateController foxStateController = other.GetComponent<FoxStateController>();
            foxStateController.TryFlee();

            FoxHealth foxHealth = other.GetComponent<FoxHealth>();
            foxHealth.TakeDamage(damage);

            Instantiate(axeHitFxPrefab, transform.position, Quaternion.identity);
            Instantiate(foxHitFxPrefab, transform.position, Quaternion.identity);

            StartCoroutine(HitCooldown());
        }
    }

    private IEnumerator HitCooldown()
    {
        canBeHit = false;
        yield return new WaitForSeconds(HIT_RATE);
        canBeHit = true;
    }
}
