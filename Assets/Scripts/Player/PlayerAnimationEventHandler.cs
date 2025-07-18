using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    private PlayerAnimationController controller;

    private void Start()
    {
        controller = GetComponentInParent<PlayerAnimationController>();
    }

    public void OnJump() { controller.OnJump(); }
    public void OnLand() { controller.OnLand(); }
}
