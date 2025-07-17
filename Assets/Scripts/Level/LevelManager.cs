using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private SheepHealth sheepHealth;
    [SerializeField] private TMP_Text timerText;

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
        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(timer);
        timerText.text = timeSpan.ToString(@"mm\:ss");
    }


    private void HandleDied()
    {
        sheepHealth.OnDied -= HandleDied;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
