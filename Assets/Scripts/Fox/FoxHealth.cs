using System;
using UnityEngine;

public class FoxHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    public event Action OnDeath;

    public int CurrentHealth { get; private set; }

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}
