using UnityEngine;

public class LevelTransitionUI : MonoBehaviour
{
    public void LoadNextScene()
    {
        LevelTransitionManager.Instance.LoadNextScene();
    }
}
