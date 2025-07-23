using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Leash : MonoBehaviour
{
    [SerializeField] private float dragDuration = 10f;
    [SerializeField] private float leashCooldownDuration = 45f;
    [SerializeField] private Image leashReloadImage;
    [SerializeField] private Image leashBackgroundReloadImage;
    [SerializeField] private Sprite leashIconSprite;
    [SerializeField] private GameObject leashStartSFXPrefab;
    [SerializeField] private GameObject leashBreakSFXPrefab;
    [SerializeField] private Transform ropeStartPoint;

    private bool canUseLeash = true;
    private float interactRange = 3f;
    private float maxDragDistance = 30f;
    private bool dragging = false;
    private bool isBreaking = false;

    private LineRenderer lineRenderer;
    private PlayerActionManager playerActionManager;

    private const float BREAK_LEASH_SFX_DURATION = 1.6f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        playerActionManager = GetComponent<PlayerActionManager>();
    }

    private void Start()
    {
        leashReloadImage.sprite = leashIconSprite;
        leashBackgroundReloadImage.sprite = leashIconSprite;
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
        
        if (!isBreaking && dragging && distanceToSheep > maxDragDistance)
        {
            Instantiate(leashBreakSFXPrefab, transform.position, Quaternion.identity);
            isBreaking = true;

            StartCoroutine(BreakLeash());
        }
    }

    private IEnumerator BreakLeash()
    {
        yield return new WaitForSeconds(BREAK_LEASH_SFX_DURATION);

        SheepManager.Instance.SheepDrag.StopDrag();

        lineRenderer.enabled = false;
        dragging = false;
        isBreaking = false;

        playerActionManager.StopAction();
    }

    private void HandleLeash(float distanceToSheep)
    {
        if (distanceToSheep <= interactRange)
        {
            playerActionManager.StartAction();
            Instantiate(leashStartSFXPrefab, transform.position, Quaternion.identity);
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

        StartCoroutine(StopLeash());

        float elapsed = 0f;
        while (elapsed < dragDuration + BREAK_LEASH_SFX_DURATION)
        {
            if (PlayerManager.Instance != null && SheepStateController.Instance != null)
            {
                Transform player = PlayerManager.Instance.transform;
                Transform sheep = SheepStateController.Instance.transform;

                lineRenderer.SetPosition(0, ropeStartPoint.position);
                lineRenderer.SetPosition(1, sheep.position);

                elapsed += Time.deltaTime;

            }
            else
            {
                yield return null;
            }

            yield return null;
        }
    }

    private IEnumerator StopLeash()
    {
        yield return new WaitForSeconds(dragDuration);
        Instantiate(leashBreakSFXPrefab, transform.position, Quaternion.identity);
        isBreaking = true;
        StartCoroutine(BreakLeash());
    }
}
