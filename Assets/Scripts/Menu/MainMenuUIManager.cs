using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public void Play()
    {
        LevelTransitionManager.Instance.TransitionOutOfScene(1);
    }
}
