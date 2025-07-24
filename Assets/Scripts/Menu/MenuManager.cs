using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject overlayCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private MenuInput menuInput;

    public bool IsOpened { get; private set; } = false;

    private void OnEnable()
    {
        OpenMenu();
        menuInput.OnGoBack += Continue;
    }

    private void OnDisable()
    {
        menuInput.OnGoBack -= Continue;
    }

    private void Update()
    {
        PlayerInputManager.Instance.LockLookInput();
    }

    private void OpenMenu()
    {
        TimeManager.Instance.SetTargetTimeScale(0f);
        overlayCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        IsOpened = true;
    }

    public void Continue()
    {
        CloseMenu();

        IsOpened = false;
    }

    private void CloseMenu()
    {
        TimeManager.Instance.SetTargetTimeScale(1f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        overlayCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Retry()
    {
        LevelTransitionManager.Instance.TransitionOutOfScene(SceneManager.GetActiveScene().buildIndex);
        CloseMenu();
    }

    public void Settings()
    {
        gameObject.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
