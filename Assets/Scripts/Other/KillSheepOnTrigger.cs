using UnityEngine;

public class KillSheepOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sheep"))
        {
            other.TryGetComponent(out SheepHealth health);
            if (health != null)
            {
                health.TakeDamage(1000);
            }
        }
    }
}
