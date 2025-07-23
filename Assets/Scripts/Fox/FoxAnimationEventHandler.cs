using System.Collections;
using UnityEngine;

public class FoxAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private FoxBiteZone foxBiteZone;
    [SerializeField] private GameObject foxAttackSFXPrefab;
    [SerializeField] private GameObject foxBiteSFXPrefab;

    public void Bite()
    {
        if (foxBiteZone.IsSheepInBiteZone && foxBiteZone.SheepHealth != null)
        {
            Instantiate(foxBiteSFXPrefab, transform.position, Quaternion.identity);
            foxBiteZone.SheepHealth.TakeDamage(foxBiteZone.GetBiteDamage());
            SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Sad);
            SheepStateController.Instance.Flee(transform);
        }
    }

    public void Attack()
    {
        Instantiate(foxAttackSFXPrefab, transform.position, Quaternion.identity);
    }
}
