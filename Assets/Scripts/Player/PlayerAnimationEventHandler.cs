using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private PetHand petHand;

    private PlayerAnimationController controller;
    private PlayerActionManager actionManager;

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
}
