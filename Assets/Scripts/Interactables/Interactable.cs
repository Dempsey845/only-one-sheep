using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum MovementType
    {
        None,
        Spin,
        PingPong
    }



    [Header("Interactable")]
    [SerializeField] private float checkRadius = 15f;
    [SerializeField] private float curiousDuration = 5f;
    [SerializeField] private float ignoreTime = 15f; // How long the interactable should be ignored for after sheep loses interes

    [Header("Movement")]
    [SerializeField] private MovementType movementType = MovementType.None;

    [SerializeField] private float spinSpeed = 50f;

    [SerializeField] private float pingPongDistance = 1f;
    [SerializeField] private float pingPongSpeed = 1f;


    private Transform sheepTranform;

    private float checkRate = 1f;
    private float timer = 0f;
    private float ignoreTimer = 0f;

    private Vector3 startPosition;

    public bool IsSheepCuriousOfThis { get; set; } = false;
    public bool IgnoreFromSheep { get; set; } = false;

    private const string SHEEP_TAG = "Sheep";

    protected SheepHealth sheepHealth;

    private void Start()
    {
        sheepTranform = SheepStateController.Instance.transform;
        startPosition = transform.position;
    }

    protected virtual void Interact()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        HandleMovement();

        if (sheepTranform == null) { return; }

        if (IgnoreFromSheep)
        {
            ignoreTimer += Time.deltaTime;

            if (ignoreTimer > ignoreTime)
            {
                IgnoreFromSheep = false;
                ignoreTimer = 0f;
            }

            return;
        }

        timer += Time.deltaTime;

        if (timer >= checkRate)
        {
            CheckIfSheepInRange();

            timer = 0f;
        }
    }

    private void CheckIfSheepInRange()
    {
        // If the Sheep isn't curious, isn't already curious of this interactable and is in range
        if (!SheepStateController.Instance.IsSheepPanicking && !SheepStateController.Instance.IsSheepCurious && !IsSheepCuriousOfThis && IsSheepInRange())
        {
            Debug.Log("Sheep is curious!");
            SheepStateController.Instance.Curious(transform.position, this, curiousDuration);
            IsSheepCuriousOfThis = true;
        }
    }

    public bool IsSheepInRange()
    {
        return Vector3.Distance(sheepTranform.position, transform.position) <= checkRadius;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(SHEEP_TAG)) { return; }

        if (!IsSheepCuriousOfThis) { return; }

        sheepHealth = collision.gameObject.GetComponent<SheepHealth>();
        Interact();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

    private void HandleMovement()
    {
        switch (movementType)
        {
            case MovementType.Spin:
                transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
                break;

            case MovementType.PingPong:
                float offset = Mathf.PingPong(Time.time * pingPongSpeed, pingPongDistance);
                transform.position = startPosition + new Vector3(0f, offset, 0f); // move up/down
                break;

            case MovementType.None:
            default:
                break;
        }
    }

}
