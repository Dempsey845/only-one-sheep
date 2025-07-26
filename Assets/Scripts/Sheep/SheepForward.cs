using UnityEngine;

public class SheepForward : MonoBehaviour
{
    private Vector3 targetPosition;
    private Transform sheepTransform;

    private void Start()
    {
        sheepTransform = SheepManager.Instance.transform;
    }

    private void Update()
    {
        if (sheepTransform == null) { return; }

        transform.position = sheepTransform.position;

        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Vector3 euler = targetRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
