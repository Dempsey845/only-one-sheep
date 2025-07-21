using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Leash : MonoBehaviour
{
    [SerializeField] private float dragDuration = 10f;
    [SerializeField] private float leashCooldownDuration = 45f;
    [SerializeField] private Image leashReloadImage;

    private bool canUseLeash = true;
    private float interactRange = 3f;
    private float maxDragDistance = 30f;
    private bool dragging = false;

    private LineRenderer lineRenderer;
    private PlayerActionManager playerActionManager;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        playerActionManager = GetComponent<PlayerActionManager>();
    }

    private void Update()
    {
        float distanceToSheep = PlayerManager.Instance.GetDistanceBetweenPlayerAndSheep();

        if (PlayerInputManager.Instance.AttackPressed && canUseLeash && !playerActionManager.IsPerformingAction)
        {
            HandleLeash(distanceToSheep);
        }

        if (!canUseLeash)
        {
            leashReloadImage.fillAmount += Time.deltaTime / leashCooldownDuration;
        } 
        
        if (dragging && distanceToSheep > maxDragDistance)
        {
            Debug.Log("Sheep to far, exiting drag");

            SheepManager.Instance.SheepDrag.StopDrag();

            lineRenderer.enabled = false;
            dragging = false;

            playerActionManager.StopAction();

        }
    }

    private void HandleLeash(float distanceToSheep)
    {
        if (distanceToSheep <= interactRange)
        {
            playerActionManager.StartAction();
            SheepStateController.Instance.Drag(dragDuration);
            SheepManager.Instance.EmojiManager.ChangeEmoji(Emoji.Annoyed);
            leashReloadImage.fillAmount = 0f;
            StartCoroutine(LeashCooldown());
            StartCoroutine(ShowLeashLine());
        }
    }

    private IEnumerator LeashCooldown()
    {
        canUseLeash = false;
        yield return new WaitForSeconds(leashCooldownDuration);
        playerActionManager.StopAction();
        canUseLeash = true;
    }

    private IEnumerator ShowLeashLine()
    {
        lineRenderer.enabled = true;
        dragging = true;

        float elapsed = 0f;
        while (elapsed < dragDuration)
        {
            Transform player = PlayerManager.Instance.transform;
            Transform sheep = SheepStateController.Instance.transform;

            lineRenderer.SetPosition(0, player.position + Vector3.up);
            lineRenderer.SetPosition(1, sheep.position + Vector3.up);

            elapsed += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
        dragging = false;
    }
}
