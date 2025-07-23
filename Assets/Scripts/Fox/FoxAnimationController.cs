using System;
using UnityEngine;

public class FoxAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlayIdleAnimation()
    {
        animator.SetTrigger("Idle");
    }

    public void PlayRunAnimation()
    {
        animator.SetTrigger("Chase");
    }

    internal void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Die");
    }
}
