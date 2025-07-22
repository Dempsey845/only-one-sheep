using UnityEngine;

public class SheepGravitateState : ISheepState
{
    private readonly float duration;

    private float timer = 0f;

    public SheepGravitateState(float duration)
    {
        this.duration = duration;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= duration)
        {
            SheepStateController.Instance.Wander();
            timer = 0f;
        }
    }
}
