using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    public bool IsPerformingAction { get; private set; }

    private float performingTime;

    private const float MAX_PERFORMING_TIME = 2f;

    private void Update()
    {
        if (IsPerformingAction)
        {
            performingTime += Time.deltaTime;

            if (performingTime > MAX_PERFORMING_TIME)
            {
                IsPerformingAction = false;
                performingTime = 0f;
            }
        }
    }

    public void StartAction()
    {
        IsPerformingAction = true;
        performingTime = 0f;
    }

    public void StopAction()
    {
        IsPerformingAction = false;
        performingTime = 0f;
    }
}
