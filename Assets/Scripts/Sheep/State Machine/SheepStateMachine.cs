using UnityEngine;

public class SheepStateMachine : MonoBehaviour
{
    private ISheepState currentState;

    public void SetState(ISheepState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter();
            Debug.Log("Entering: ");
            Debug.Log(newState);
        }
    }

    private void Update()
    {
        currentState?.Update();
    }

    public ISheepState GetCurrentState() { return currentState; }
}
