using UnityEngine;

public class FoxStateMachine : MonoBehaviour
{
    private IFoxState currentState;

    public void SetState(IFoxState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter();
            Debug.Log("Fox Entering: ");
            Debug.Log(newState);
        }
    }

    private void Update()
    {
        currentState?.Update();
    }

    public IFoxState GetCurrentState() { return currentState; }
}
