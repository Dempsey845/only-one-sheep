using TMPro;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private TMP_Text levelCompleteText;

    private void Start()
    {
        levelCompleteText.text = "Time taken: " + GameManager.Instance.PreviousLevelTime;
    }
}
