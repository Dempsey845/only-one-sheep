using UnityEngine;

public class FoxBiteZone : MonoBehaviour
{
    [SerializeField] private int biteDamage = 5;

    public bool IsSheepInBiteZone { get; private set; }
    public SheepHealth SheepHealth { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Sheep")) { return; }

        IsSheepInBiteZone = true;
        SheepHealth = other.GetComponent<SheepHealth>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Sheep")) { return; }

        IsSheepInBiteZone = false;
    }

    public int GetBiteDamage() { return biteDamage; }
}
