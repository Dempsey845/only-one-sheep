using System;
using UnityEngine;

public class FoxHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject foxHurtSFXPrefab;
    [SerializeField] private Collider mainCollider;

    private FoxStateController controller;
    private AudioSource audioSource;

    public event Action OnDeath;

    public int CurrentHealth { get; private set; }

    private void Start()
    {
        CurrentHealth = maxHealth;
        controller = GetComponent<FoxStateController>();
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        Instantiate(foxHurtSFXPrefab, transform.position, Quaternion.identity);

        if (CurrentHealth <= 0)
        {
            controller.Die();
            mainCollider.enabled = false;
            audioSource.loop = false;
            audioSource.Stop();
            OnDeath?.Invoke();
        }
    }
}
