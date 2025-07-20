using UnityEngine;

public class SheepManager : MonoBehaviour
{
    [SerializeField] private Transform sheepCanvasPoint;

    public static SheepManager Instance { get; private set; }

    public SheepEmojiManager EmojiManager { get; private set; }

    public SheepRagdollController RagdollController { get; private set; }

    public SheepStateMachine StateMachine { get; private set; }
    public SheepDrag SheepDrag { get; private set; }

    private void Awake()
    {
        Instance = this;

        EmojiManager = GetComponent<SheepEmojiManager>();

        RagdollController = GetComponent<SheepRagdollController>();

        StateMachine = GetComponent<SheepStateMachine>();

        SheepDrag = GetComponent<SheepDrag>();
    }

    public Vector3 GetSheepCanvasPosition() { return sheepCanvasPoint.position; }
}
