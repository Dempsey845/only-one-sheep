using System.Collections;
using UnityEngine;

public class AxeCollision : MonoBehaviour
{
    private bool canBeHit = true;

    private const float HIT_RATE = 0.3f;

    private void OnTriggerEnter(Collider other)
    {
        if (!canBeHit) { return; }

        if (other.CompareTag("Fox"))
        {
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
