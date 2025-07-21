using UnityEngine;

public class PlayerActionManager : MonoBehaviour
{
    public bool IsPerformingAction { get; private set; }

    public void StartAction()
    {
        IsPerformingAction = true;
    }

    public void StopAction()
    {
        IsPerformingAction = false;
    }
}
