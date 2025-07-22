using UnityEngine;

public class PetHand : MonoBehaviour
{
    [SerializeField] private Pet pet;
    [SerializeField] private GameObject petFx;
    [SerializeField] private float petRadius = 0.5f;
    [SerializeField] private LayerMask sheepLayer;
    private bool isPetting = false;

    private void Update()
    {
        if (isPetting)
        {
            if (Physics.OverlapSphere(transform.position, petRadius, sheepLayer).Length > 0)
            {
                TryPet();
            }
        }
    }

    private void TryPet()
    {
        if (pet.TryPetSheep())
        {
            Instantiate(petFx, transform.position, Quaternion.identity);
            StopPet();
        }
    }

    public void StartPet()
    {
        isPetting = true;
    }

    public void StopPet()
    {
        isPetting = false;
    }
}