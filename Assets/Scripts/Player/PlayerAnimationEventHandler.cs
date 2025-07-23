using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private PetHand petHand;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private GameObject axeSwingSFXPrefab;

    private PlayerAnimationController controller;
    private PlayerActionManager actionManager;

    private AudioSource audioSource;

    private void Start()
    {
        controller = GetComponentInParent<PlayerAnimationController>();
        actionManager = GetComponentInParent<PlayerActionManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void OnJump() { controller.OnJump(); }
    public void OnLand() { controller.OnLand(); }

    public void StopAction() { actionManager.StopAction(); }

    public void StopPet() { petHand.StopPet(); }
    public void StartPet() { petHand.StartPet(); }

    public void PlayFootstepSound()
    {
        AudioClip randomClip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.PlayOneShot(randomClip);
    }

    public void PlayAxeSwingSFX()
    {
        Instantiate(axeSwingSFXPrefab, transform.position, Quaternion.identity);
    }
}
