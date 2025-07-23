using System;
using UnityEngine;
using UnityEngine.AI;

public class Barn : MonoBehaviour
{
    [SerializeField] private GameObject billboard;

    private Animator animator;
    private NavMeshObstacle obstacle;

    private void Start()
    {
        animator = GetComponent<Animator>();
        obstacle = GetComponent<NavMeshObstacle>();

        billboard.SetActive(false);

        PlayerManager.Instance.OnPickupKey += HandlePickupKey;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.OnPickupKey -= HandlePickupKey;
    }

    private void HandlePickupKey()
    {
        billboard.SetActive(true);
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
