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
    [SerializeField] private GameObject whistleSFXPrefab;
    
    private SheepStateController sheepStateController;
    private PlayerActionManager playerActionManager;

    private bool canWhistle = true;

    public event Action OnPerformedWhistle;

    private void Awake()
    {
        playerActionManager = GetComponent<PlayerActionManager>();
    }

    private void Start()
    {
        sheepStateController = SheepStateController.Instance;
    }

    private void Update()
    {
        if (PlayerInputManager.Instance.WhistlePressed && !playerActionManager.IsPerformingAction)
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

        Instantiate(whistleSFXPrefab, transform.position, Quaternion.identity);

        StartCoroutine(WhistleCooldown());

        playerActionManager.StartAction();
        OnPerformedWhistle?.Invoke();

        if (distanceFromSheep > maxWhistleDistance) { return; }

        bool chase = distanceFromSheep > panicDistance;

        if (chase) 
        {
            sheepStateController.ChasePlayer(chaseDuration);

            SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Annoyed);
            SheepManager.Instance.EnqueueRandomSheepClip();
        }
        else 
        { 
            sheepStateController.Panic(panicDuration);

            SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Angry);
            SheepManager.Instance.EnqueueRandomSheepClip();
        }
    }

    private IEnumerator WhistleCooldown()
    {
        canWhistle = false;
        yield return new WaitForSeconds(whistleCooldownDuration);
        canWhistle = true;
    }
}
