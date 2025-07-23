using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private SheepHealth sheepHealth;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private int levelCompleteBuildIndex = 2;
    [SerializeField] private int nextLevelBuildIndex = 0;

    private float timer;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        sheepHealth.OnDied += HandleDied;
    }

    private void OnDisable()
    {
        sheepHealth.OnDied -= HandleDied;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        timerText.text = FormatTimeString();
    }

    private string FormatTimeString()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        return timeSpan.ToString(@"mm\:ss");
    }

    private void HandleDied()
    {
        sheepHealth.OnDied -= HandleDied;

        int nextSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        LevelTransitionManager.Instance.TransitionOutOfScene(nextSceneBuildIndex);
    }

    public void CompleteLevel()
    {
        GameManager.Instance.PreviousLevelTime = FormatTimeString();
        GameManager.Instance.NextLevelBuildIndex = nextLevelBuildIndex;

        LevelTransitionManager.Instance.TransitionOutOfScene(levelCompleteBuildIndex);
    }
}
