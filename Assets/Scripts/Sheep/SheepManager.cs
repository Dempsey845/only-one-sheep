using System.Collections;
using UnityEngine;

public class SheepManager : MonoBehaviour
{
    [SerializeField] private Transform sheepCanvasPoint;
    [SerializeField] private ParticleSystem gravitateFX;

    public static SheepManager Instance { get; private set; }

    public SheepEmojiManager EmojiManager { get; private set; }

    public SheepRagdollController RagdollController { get; private set; }

    public SheepStateMachine StateMachine { get; private set; }
    public SheepDrag SheepDrag { get; private set; }

    private ParticleSystem.EmissionModule emissionModule;

    private void Awake()
    {
        Instance = this;

        EmojiManager = GetComponent<SheepEmojiManager>();

        RagdollController = GetComponent<SheepRagdollController>();

        StateMachine = GetComponent<SheepStateMachine>();

        SheepDrag = GetComponent<SheepDrag>();

        emissionModule = gravitateFX.emission;
    }

    private void Start()
    {
        emissionModule.enabled = false;
    }

    public Vector3 GetSheepCanvasPosition() { return sheepCanvasPoint.position; }

    public void PlayGravitateFX(float duration)
    {
        emissionModule.enabled = true;
        StartCoroutine(StopGravitateFX(duration));
    }

    public IEnumerator StopGravitateFX(float duration)
    {
        yield return new WaitForSeconds(duration);
        emissionModule.enabled = false;
    }
}
