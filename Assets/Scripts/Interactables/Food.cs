using UnityEngine;

public class Food : Interactable
{
    [Header("Food")]
    [SerializeField] private float panicDuration = 10f;
    [SerializeField] private int sheepDamage = 5;

    protected override void Interact()
    {
        if (SheepManager.Instance == null || SheepStateController.Instance == null) { return; }

        SheepStateController.Instance.Panic(panicDuration);

        sheepHealth.TakeDamage(sheepDamage);

        SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Sick);

        base.Interact();
    }
}
