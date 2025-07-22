using UnityEngine;
using UnityEngine.AI;

public class Barn : MonoBehaviour
{
    private Animator animator;
    private NavMeshObstacle obstacle;

    private void Start()
    {
        animator = GetComponent<Animator>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && PlayerManager.Instance.HasKey)
        {
            animator.SetTrigger("Open");
            obstacle.enabled = false;
        }
    }
}
