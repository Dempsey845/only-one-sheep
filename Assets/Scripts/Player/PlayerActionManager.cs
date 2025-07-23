using UnityEngine;

public enum PrimaryActions
{
    None,
    Axe,
    Leash
}

public class PlayerActionManager : MonoBehaviour
{
    [SerializeField] private PrimaryActions primaryAction = PrimaryActions.Leash;

    public bool IsPerformingAction { get; private set; }

    private float performingTime;

    private const float MAX_PERFORMING_TIME = 2f;

    private void Awake()
    {
        GetComponent<Leash>().enabled = primaryAction == PrimaryActions.Leash;
        GetComponent<Axe>().enabled = primaryAction == PrimaryActions.Axe;
    }

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
