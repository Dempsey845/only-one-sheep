using UnityEngine;

public class Food : Interactable
{
    [SerializeField] private float panicDuration = 10f;

    protected override void Interact()
    {
        SheepStateController.Instance.Panic(panicDuration);
        base.Interact();
    }
}
