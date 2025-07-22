using System.Collections;
using UnityEngine;

public class FoxAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private FoxBiteZone foxBiteZone;

    public void Bite()
    {
        if (foxBiteZone.IsSheepInBiteZone && foxBiteZone.SheepHealth != null)
        {
            foxBiteZone.SheepHealth.TakeDamage(foxBiteZone.GetBiteDamage());
            SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Sad);
            SheepStateController.Instance.Flee(transform);
        }
    }
}
