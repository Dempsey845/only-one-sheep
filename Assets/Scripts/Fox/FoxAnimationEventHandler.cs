using System.Collections;
using UnityEngine;

public class FoxAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private FoxBiteZone foxBiteZone;

    private FoxAgent agent;
    private FoxAnimationController controller;

    private const float ATTACK_ANIMATION_DURATION = .7f;

    private void Start()
    {
        agent = GetComponentInParent<FoxAgent>();
        controller = GetComponentInParent<FoxAnimationController>();
    }

    public void Bite()
    {
        if (foxBiteZone.IsSheepInBiteZone && foxBiteZone.SheepHealth != null)
        {
            foxBiteZone.SheepHealth.TakeDamage(foxBiteZone.GetBiteDamage());
            SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Sad);
            StartCoroutine(agent.StopMovement(ATTACK_ANIMATION_DURATION));
            StartCoroutine(RestoreMovementAnimation());
        }
    }

    public IEnumerator RestoreMovementAnimation()
    {
        yield return new WaitForSeconds(ATTACK_ANIMATION_DURATION);
        controller.PlayRunAnimation();
    }
}
