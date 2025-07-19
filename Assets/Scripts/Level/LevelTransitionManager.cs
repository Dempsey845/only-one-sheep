using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionManager : MonoBehaviour
{
    [SerializeField] private Animator levelTransitionAnimator;

    public static LevelTransitionManager Instance;

    private int nextSceneBuildIndex;

    private const string FADE_OUT_TRIGGER = "Fade Out";

    private void Awake()
    {
        Instance = this;
    }

    public void TransitionOutOfScene(int nextSceneBuildIndex)
    {
        this.nextSceneBuildIndex = nextSceneBuildIndex;
        levelTransitionAnimator.SetTrigger(FADE_OUT_TRIGGER);
    }
    
    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneBuildIndex);
    }
}
