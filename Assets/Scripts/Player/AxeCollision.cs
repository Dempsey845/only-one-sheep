using System.Collections;
using UnityEngine;

public class AxeCollision : MonoBehaviour
{
    [SerializeField] private int damage = 1;

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

            Debug.Log("Hit fox!");

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
