using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

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
        return Vector3.Distance(transform.position, sheepTransform.position);
    }
}
