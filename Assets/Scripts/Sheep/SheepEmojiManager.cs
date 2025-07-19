using UnityEngine;
using System.Collections.Generic;

public enum Emoji
{
    None = -1,
    Confused = 0,
    Angry = 1,
    Happy = 2,
    Sad = 3,
    Annoyed = 4,
    Tired = 5,
    Love = 6,
    Sick = 7,
}

public class SheepEmojiManager : MonoBehaviour
{
    [SerializeField] private Sprite[] emojiesInOrder;

    private SheepCanvas sheepCanvas;

    private Queue<(Emoji emoji, float duration)> emojiQueue = new();
    private float timer = 0f;
    private float currentEmojiMinDuration = 0f;

    private void Start()
    {
        sheepCanvas = FindFirstObjectByType<SheepCanvas>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > currentEmojiMinDuration && emojiQueue.Count > 0)
        {
            var (nextEmoji, minDuration) = emojiQueue.Dequeue();
            sheepCanvas.SetEmojiSprite(emojiesInOrder[(int)nextEmoji]);
            currentEmojiMinDuration = minDuration;
            timer = 0f;
        }
    }

    public void ChangeEmoji(Emoji emoji, float minDuration = 1f)
    {
        emojiQueue.Enqueue((emoji, minDuration));
    }
}
