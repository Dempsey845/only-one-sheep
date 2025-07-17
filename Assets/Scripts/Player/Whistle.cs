using System.Collections;
using UnityEngine;

public class Whistle : MonoBehaviour
{
    [SerializeField] private float whistleCooldownDuration = 5f;
    [SerializeField] private float chaseDuration = 15f;
    [SerializeField] private float panicDuration = 10f;
    [SerializeField] private float panicDistance = 10f;
    [SerializeField] private float maxWhistleDistance = 25f;
    
    private SheepStateController sheepStateController;
    private Transform sheepTransform;

    private bool canWhistle = true;

    private void Start()
    {
        sheepStateController = SheepStateController.Instance;
        sheepTransform = sheepStateController.transform;
    }

    private void Update()
    {
        if (PlayerInputManager.Instance.WhistlePressed)
        {
            HandleWhistle();
        }
    }

    private void HandleWhistle()
    {
        if (!canWhistle) { return; }

        float distanceFromSheep = GetDistanceBetweenPlayerAndSheep();

        if (distanceFromSheep > maxWhistleDistance) { return; }

        bool chase = distanceFromSheep > panicDistance;

        if (chase) { sheepStateController.ChasePlayer(chaseDuration); }
        else { sheepStateController.Panic(panicDuration); }

        StartCoroutine(WhistleCooldown());
    }

    private float GetDistanceBetweenPlayerAndSheep()
    {
        return Vector3.Distance(transform.position, sheepTransform.position);
    }

    private IEnumerator WhistleCooldown()
    {
        canWhistle = false;
        yield return new WaitForSeconds(whistleCooldownDuration);
        canWhistle = true;
    }
}
