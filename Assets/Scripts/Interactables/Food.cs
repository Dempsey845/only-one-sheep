using UnityEngine;

public class Food : Interactable
{
    [SerializeField] private float panicDuration = 10f;
    [SerializeField] private int sheepDamage = 5;

    protected override void Interact()
    {
        SheepStateController.Instance.Panic(panicDuration);
        sheepHealth.TakeDamage(sheepDamage);
        base.Interact();
    }
}
