using UnityEngine;

public class FoxChaseState : IFoxState
{
    private readonly FoxStateController stateController;
    private readonly FoxAnimationController animationController;
    private readonly FoxAgent agent;
    private readonly float duration;

    private float timer;
    private bool canAttack = true;
    private float attackTimer = 0f;

    private const float ATTACK_COOLDOWN = 1f;

    public FoxChaseState(FoxStateController stateController, FoxAnimationController animationController, FoxAgent agent, float duration)
    {
        this.stateController = stateController;
        this.animationController = animationController;
        this.agent = agent;
        this.duration = duration;
    }

    public void Enter()
    {
        agent.StartChasingSheep();
        animationController.PlayRunAnimation();
        agent.OnFoxReachedSheepDuringChase += HandleFoxAttack;
    }

    public void Exit()
    {
        agent.StopChasingSheep();
        agent.OnFoxReachedSheepDuringChase -= HandleFoxAttack;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer > duration)
        {
            stateController.Idle();
            timer = 0;
        }

        if (!canAttack)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= ATTACK_COOLDOWN)
            {
                canAttack = true;
                attackTimer = 0f;
            }
        }
    }

    private void HandleFoxAttack()
    {
        if (!canAttack) { return; }

        animationController.PlayAttackAnimation();

        canAttack = false;
    }
}
