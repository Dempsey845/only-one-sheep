using UnityEngine;

public class PlayerThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -4);
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minY = -20f;
    [SerializeField] private float maxY = 60f;

    private float currentYaw = 0f;
    private float currentPitch = 10f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        // Read mouse input
        float mouseX = PlayerInputManager.Instance.LookInput.x;
        float mouseY = PlayerInputManager.Instance.LookInput.y;

        currentYaw += mouseX * rotationSpeed;
        currentPitch -= mouseY * rotationSpeed;
        currentPitch = Mathf.Clamp(currentPitch, minY, maxY);

        // Convert yaw/pitch to camera position
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = desiredPosition;

        // Look at target
        transform.LookAt(target.position + Vector3.up * 1.5f); // Slight height offset for head
    }
}
