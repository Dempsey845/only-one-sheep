using System;
using UnityEngine;

public class Shed : MonoBehaviour
{
    [SerializeField] private ExplosiveBarrel explosiveBarrel;
    [SerializeField] private GameObject shedExplodeFXPrefab;
    [SerializeField] private FoxHealth foxHealth;

    private void OnEnable()
    {
        explosiveBarrel.OnExplode += HandleExplosion;
        foxHealth.OnDeath += HandleFoxDeath;
    }

    private void OnDisable()
    {
        explosiveBarrel.OnExplode -= HandleExplosion;
        foxHealth.OnDeath -= HandleFoxDeath;
    }

    private void Start()
    {
        explosiveBarrel.IsInteractable = false;
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
}
