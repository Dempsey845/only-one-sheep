using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private PetHand petHand;
    [SerializeField] private GameObject[] footstepSFXPrefabs;
    [SerializeField] private GameObject axeSwingSFXPrefab;

    private PlayerAnimationController controller;
    private PlayerActionManager actionManager;

    private int lastFootstep = 0;

    private void Start()
    {
        controller = GetComponentInParent<PlayerAnimationController>();
        actionManager = GetComponentInParent<PlayerActionManager>();
    }

    public void OnJump() { controller.OnJump(); }
    public void OnLand() { controller.OnLand(); }

    public void StopAction() { actionManager.StopAction(); }

    public void StopPet() { petHand.StopPet(); }
    public void StartPet() { petHand.StartPet(); }

    public void PlayFootstepSound()
    {
        GameObject randomSFX = footstepSFXPrefabs[lastFootstep];
        Instantiate(randomSFX, transform.position, Quaternion.identity);
        lastFootstep++;
        if (lastFootstep >= footstepSFXPrefabs.Length)
        {
            lastFootstep = 0;
        }
    }

    public void PlayAxeSwingSFX()
    {
        Instantiate(axeSwingSFXPrefab, transform.position, Quaternion.identity);
    }
}
