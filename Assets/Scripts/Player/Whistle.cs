using System.Collections;
using UnityEngine;

public class Whistle : MonoBehaviour
{
    [SerializeField] private float whistleCooldownDuration = 5f;
    [SerializeField] private float chaseDuration = 5f;
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

        sheepStateController.ChasePlayer(chaseDuration);

        StartCoroutine(WhistleCooldown());
    }

    private IEnumerator WhistleCooldown()
    {
        canWhistle = false;
        yield return new WaitForSeconds(whistleCooldownDuration);
        canWhistle = true;
    }
}
