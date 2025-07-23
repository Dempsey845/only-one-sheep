using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepManager : MonoBehaviour
{
    [SerializeField] private Transform sheepCanvasPoint;
    [SerializeField] private ParticleSystem gravitateFX;

    [Header("Audio")]
    [SerializeField] private AudioClip[] sheepClips;
    [SerializeField] private float playRandomClipRate = 5f;

    public static SheepManager Instance { get; private set; }

    public SheepEmojiManager EmojiManager { get; private set; }
    public SheepRagdollController RagdollController { get; private set; }
    public SheepStateMachine StateMachine { get; private set; }
    public SheepDrag SheepDrag { get; private set; }

    private ParticleSystem.EmissionModule emissionModule;
    private AudioSource audioSource;

    private float clipTimer;

    private Queue<AudioClip> audioQueue = new Queue<AudioClip>();
    private bool isPlayingAudio = false;

    private void Awake()
    {
        Instance = this;

        EmojiManager = GetComponent<SheepEmojiManager>();
        RagdollController = GetComponent<SheepRagdollController>();
        StateMachine = GetComponent<SheepStateMachine>();
        SheepDrag = GetComponent<SheepDrag>();
        audioSource = GetComponent<AudioSource>();
        emissionModule = gravitateFX.emission;
    }

    private void Start()
    {
        emissionModule.enabled = false;
        StartCoroutine(AudioPlaybackRoutine());
    }

    private void Update()
    {
        clipTimer += Time.deltaTime;

        if (clipTimer >= playRandomClipRate)
        {
            EnqueueRandomSheepClip();
            clipTimer = 0f;
        }
    }

    public Vector3 GetSheepCanvasPosition() => sheepCanvasPoint.position;

    public void PlayGravitateFX(float duration)
    {
        emissionModule.enabled = true;
        StartCoroutine(StopGravitateFX(duration));
    }

    private IEnumerator StopGravitateFX(float duration)
    {
        yield return new WaitForSeconds(duration);
        emissionModule.enabled = false;
    }

    public void EnqueueRandomSheepClip()
    {
        AudioClip randomClip = sheepClips[Random.Range(0, sheepClips.Length)];
        audioQueue.Enqueue(randomClip);
    }

    private IEnumerator AudioPlaybackRoutine()
    {
        while (true)
        {
            if (audioQueue.Count > 0 && !isPlayingAudio)
            {
                isPlayingAudio = true;

                AudioClip clip = audioQueue.Dequeue();
                audioSource.PlayOneShot(clip);

                yield return new WaitForSeconds(clip.length + 1f); 

                isPlayingAudio = false;
            }

            yield return null;
        }
    }
}
