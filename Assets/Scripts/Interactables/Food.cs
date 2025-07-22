using UnityEngine;

public class Food : Interactable
{
    [Header("Food")]
    [SerializeField] private float panicDuration = 10f;
    [SerializeField] private int sheepDamage = 5;
    [SerializeField] private GameObject interactFX;

    protected override void Interact()
    {
        if (SheepManager.Instance == null || SheepStateController.Instance == null) { return; }

        SheepStateController.Instance.Panic(panicDuration);

        sheepHealth.TakeDamage(sheepDamage);

        SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Sick);

        Instantiate(interactFX, transform.position, Quaternion.identity);

        base.Interact();
    }
}
