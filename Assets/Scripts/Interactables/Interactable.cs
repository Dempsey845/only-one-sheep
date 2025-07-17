using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float checkRadius = 15f;
    [SerializeField] private float curiousDuration = 5f;
    [SerializeField] private float ignoreTime = 15f; // How long the interactable should be ignored for after sheep loses interes

    private Transform sheepTranform;


    private float checkRate = 1f;
    private float timer = 0f;
    private float ignoreTimer = 0f;

    public bool IsSheepCuriousOfThis { get; set; } = false;
    public bool IgnoreFromSheep { get; set; } = false;

    private const string SHEEP_TAG = "Sheep";

    private void Start()
    {
        sheepTranform = SheepStateController.Instance.transform; 
    }

    protected virtual void Interact()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
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

        Interact();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

}
