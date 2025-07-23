using UnityEngine;

public class Key : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float pingPongSpeed = 5f;
    [SerializeField] private float pingPongDistance = 0.5f;
    [SerializeField] private float spinSpeed = 25f;
    [SerializeField] private GameObject pickupSFXPrefab;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }

        PlayerManager.Instance.OnPickupKey();

        Instantiate(pickupSFXPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void HandleMovement()
    {
        float offset = Mathf.PingPong(Time.time * pingPongSpeed, pingPongDistance);
        transform.position = startPosition + new Vector3(0f, offset, 0f);

        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
