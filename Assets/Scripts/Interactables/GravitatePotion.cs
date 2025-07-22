using UnityEngine;

public class GravitatePotion : Interactable
{
    [Header("Gravitate")]
    [SerializeField] private float duration = 10f;

    protected override void Interact()
    {
        SheepStateController.Instance.Gravitate(duration);
        SheepManager.Instance.PlayGravitateFX(duration);
        SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Sick);
        base.Interact();
    }
}