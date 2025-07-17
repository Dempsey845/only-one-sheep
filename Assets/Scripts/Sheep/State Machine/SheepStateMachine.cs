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
            currentState.Enter();
    }

    private void Update()
    {
        currentState?.Update();
    }
}
