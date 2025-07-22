using System;
using UnityEngine;

public class Shed : Interactable
{
    [Header("Shed")]
    [SerializeField] private ExplosiveBarrel explosiveBarrel;
    [SerializeField] private GameObject shedExplodeFXPrefab;
    [SerializeField] private GameObject foxPrefab;
    [SerializeField] private Transform foxHome;
    
    private FoxHealth foxHealth;

    protected override void Start()
    {
        explosiveBarrel.IsInteractable = false;
        base.Start();
    }

    private void OnDisable()
    {
        explosiveBarrel.OnExplode -= HandleExplosion;
        if (foxHealth != null) { foxHealth.OnDeath -= HandleFoxDeath; }
    }

    private void HandleExplosion()
    {
        Instantiate(shedExplodeFXPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void HandleFoxDeath()
    {
        explosiveBarrel.IsInteractable = true;
    }

    protected override void Interact()
    {
        if (!IsInteractable) { return; }

        FoxManager foxManager = Instantiate(foxPrefab, foxHome.position, Quaternion.identity).GetComponent<FoxManager>();
        foxManager.Init(foxHome);

        foxHealth = foxManager.FoxHealth;

        explosiveBarrel.OnExplode += HandleExplosion;
        foxHealth.OnDeath += HandleFoxDeath;

        IsInteractable = false;
    }
}
