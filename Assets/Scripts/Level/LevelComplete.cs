using System.Collections;
using TMPro;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private TMP_Text levelCompleteText;

    private void Start()
    {
        if (GameManager.Instance != null) { levelCompleteText.text = "Time taken: " + GameManager.Instance.PreviousLevelTime; }

        StartCoroutine(BackToMainMenu());
    }

    private IEnumerator BackToMainMenu()
    {
        yield return new WaitForSeconds(5f);
        LevelTransitionManager.Instance.TransitionOutOfScene(0);
    }
}
