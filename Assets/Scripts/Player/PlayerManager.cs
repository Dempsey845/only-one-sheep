using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public bool HasKey { get; private set; }

    private Transform sheepTransform;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        sheepTransform = SheepStateController.Instance.transform;
        Application.targetFrameRate = 60;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float GetDistanceBetweenPlayerAndSheep()
    {
        if (transform.position == null || sheepTransform == null)
        {
            return 0f;
        }

        return Vector3.Distance(transform.position, sheepTransform.position);
    }

    public void OnPickupKey()
    {
        HasKey = true;
    }
}
