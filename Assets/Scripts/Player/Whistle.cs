using System.Collections;
using UnityEngine;

public class Whistle : MonoBehaviour
{
    [SerializeField] private float whistleCooldownDuration = 5f;
    [SerializeField] private float chaseDuration = 15f;
    [SerializeField] private float panicDuration = 10f;
    [SerializeField] private SheepStateController sheepStateController; // for testing only

    private bool canWhistle = true;

    private void Update()
    {
        if (PlayerInputManager.Instance.WhistlePressed)
        {
            HandleWhistle();
        }
    }

    private void HandleWhistle()
    {
        if (!canWhistle) return;

        bool chase = Random.Range(0, 2) == 1;

        if (chase) sheepStateController.ChasePlayer(chaseDuration);
        else sheepStateController.Panic(panicDuration);

            StartCoroutine(WhistleCooldown());
    }

    private IEnumerator WhistleCooldown()
    {
        canWhistle = false;
        yield return new WaitForSeconds(whistleCooldownDuration);
        canWhistle = true;
    }
}
