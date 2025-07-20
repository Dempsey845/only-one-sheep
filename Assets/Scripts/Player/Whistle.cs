using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Whistle : MonoBehaviour
{
    [SerializeField] private float whistleCooldownDuration = 5f;
    [SerializeField] private float chaseDuration = 15f;
    [SerializeField] private float panicDuration = 10f;
    [SerializeField] private float panicDistance = 10f;
    [SerializeField] private float maxWhistleDistance = 25f;
    [SerializeField] private Image whistleReloadImage;
    
    private SheepStateController sheepStateController;

    private bool canWhistle = true;

    public event Action OnPerformedWhistle;

    private void Start()
    {
        sheepStateController = SheepStateController.Instance;
    }

    private void Update()
    {
        if (PlayerInputManager.Instance.WhistlePressed)
        {
            HandleWhistle();
        }

        if (!canWhistle)
        {
            whistleReloadImage.fillAmount += Time.deltaTime / whistleCooldownDuration;
        }
    }

    private void HandleWhistle()
    {
        if (!canWhistle) { return; }

        float distanceFromSheep = PlayerManager.Instance.GetDistanceBetweenPlayerAndSheep();

        whistleReloadImage.fillAmount = 0;

        StartCoroutine(WhistleCooldown());

        if (distanceFromSheep > maxWhistleDistance) { return; }

        bool chase = distanceFromSheep > panicDistance;

        if (chase) 
        {
            sheepStateController.ChasePlayer(chaseDuration);

            SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Annoyed);
        }
        else 
        { 
            sheepStateController.Panic(panicDuration);

            SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Angry);
        }

        OnPerformedWhistle?.Invoke();
    }

    private IEnumerator WhistleCooldown()
    {
        canWhistle = false;
        yield return new WaitForSeconds(whistleCooldownDuration);
        canWhistle = true;
    }
}
