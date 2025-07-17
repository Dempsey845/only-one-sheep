using UnityEngine;
using System;

public class SheepHealth : MonoBehaviour
{
    [SerializeField] private int startHealth = 100;
    [SerializeField] private int maxHealth = 100;

    public int CurrentHealth { get; private set; }

    public event Action<int> OnDamaged;       
    public event Action<int> OnHealed;        
    public event Action OnDied;

    private void Awake()
    {
        CurrentHealth = startHealth;
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth -= damage;
        OnDamaged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            HandleDeath();
        }
    }

    public void HealSheep(int healAmount)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth += healAmount;

        if (CurrentHealth > maxHealth)
        {
            CurrentHealth = maxHealth;
        }

        OnHealed?.Invoke(CurrentHealth);
    }

    private void HandleDeath()
    {
        OnDied?.Invoke();
        Destroy(gameObject);
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
