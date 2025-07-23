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

    private SheepManager manager;

    private void Awake()
    {
        CurrentHealth = startHealth;
        manager = GetComponent<SheepManager>();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        TakeDamage(10);
    //    }
    //}

    public void TakeDamage(int damage)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth -= damage;
        OnDamaged?.Invoke(CurrentHealth);

        manager.EnqueueRandomSheepClip();

        if (CurrentHealth <= 0)
        {
            HandleDeath();
        }
    }

    public void HealSheep(int healAmount)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth += healAmount;

        manager.EnqueueRandomSheepClip();

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
