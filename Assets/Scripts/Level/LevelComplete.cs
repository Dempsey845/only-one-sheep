using System.Collections;
using TMPro;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private TMP_Text levelCompleteText;
    
    private void Start()
    {
        if (GameManager.Instance != null) { levelCompleteText.text = "Time taken: " + GameManager.Instance.PreviousLevelTime; }

        StartCoroutine(LoadNextLevel(GameManager.Instance.NextLevelBuildIndex));
    }

    private IEnumerator LoadNextLevel(int nextLevelBuildIndex)
    {
        yield return new WaitForSeconds(5f);
        LevelTransitionManager.Instance.TransitionOutOfScene(nextLevelBuildIndex);
    }
}
