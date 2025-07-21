using UnityEngine;

public class FoxAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlayIdle()
    {
        animator.SetTrigger("Idle");
    }

    public void PlayChase()
    {
        animator.SetTrigger("Chase");
    }
}
