using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Axe : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private Image attackReloadImage;

    private bool canAttack = true;

    public event Action OnPerformedAxe;

    private PlayerActionManager playerActionManager;

    private void Awake()
    {
        playerActionManager = GetComponent<PlayerActionManager>();
    }

    private void Update()
    {
        if (PlayerInputManager.Instance.AttackPressed && canAttack && !playerActionManager.IsPerformingAction)
        {
            playerActionManager.StartAction();

            attackReloadImage.fillAmount = 0f;
            OnPerformedAxe?.Invoke();
            StartCoroutine(AttackCooldown());
        }

        if (!canAttack)
        {
            attackReloadImage.fillAmount += Time.deltaTime / attackCooldown;
        }
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
