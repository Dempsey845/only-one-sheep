using UnityEngine;

public class FakeObstacle : MonoBehaviour
{
    /* Prevents the sheep from walking into the pen by itself */

    [SerializeField] private GameObject fakeObstacle;

    private SheepStateMachine stateMachine;

    private float checkStateOfSheepRate = 0.5f;
    private float timer;

    private void Start()
    {
        if (fakeObstacle == null)
        {
            Destroy(this);
        }

        stateMachine = FindFirstObjectByType<SheepStateMachine>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > checkStateOfSheepRate)
        {
            HandleFakeObstacle();
            timer = 0;
        }
    }

    private void HandleFakeObstacle()
    {
        if (stateMachine == null) { return; }

        ISheepState currentState = stateMachine.GetCurrentState();

        fakeObstacle.SetActive(currentState is not SheepFollowState);
    }
}
