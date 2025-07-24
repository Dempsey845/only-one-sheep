using UnityEngine;

public class SettingsUIManager : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject optionsPage;
    [SerializeField] private GameObject audioPage;
    [SerializeField] private GameObject graphicsPage;
    [SerializeField] private GameObject controlsPage;
    [SerializeField] private MenuInput menuInput;

    private void OnEnable()
    {
        OpenOptionsPage();
    }

    private void OnDisable()
    {
        menuInput.OnGoBack -= CloseSettings;
    }

    private void Update()
    {
        PlayerInputManager.Instance.LockLookInput();
    }

    public void CloseSettings()
    {
        gameObject.SetActive(false);
        menuCanvas.SetActive(true);
    }

    public void CloseOptionsPage()
    {
        optionsPage.SetActive(false);
        menuInput.OnGoBack -= CloseSettings;
    }

    public void OpenOptionsPage()
    {
        optionsPage.SetActive(true);
        menuInput.OnGoBack += CloseSettings;
    }

    public void OpenAudioSettings()
    {
        audioPage.SetActive(true);
        CloseOptionsPage();
        menuInput.OnGoBack += CloseAudioSettings;
    }

    public void CloseAudioSettings()
    {
        audioPage.SetActive(false);
        menuInput.OnGoBack -= CloseAudioSettings;
        OpenOptionsPage();
    }

    public void OpenGraphicsSettings()
    {
        graphicsPage.SetActive(true);
        CloseOptionsPage();
        menuInput.OnGoBack += CloseGraphicsSettings;
    }

    public void CloseGraphicsSettings()
    {
        graphicsPage.SetActive(false);
        menuInput.OnGoBack -= CloseGraphicsSettings;
        OpenOptionsPage();
    }

    public void OpenControlsSettings()
    {
        controlsPage.SetActive(true);
        CloseOptionsPage();
        menuInput.OnGoBack += CloseControlsSettings;
    }

    public void CloseControlsSettings()
    {
        controlsPage.SetActive(false);
        menuInput.OnGoBack -= CloseControlsSettings;
        OpenOptionsPage();
    }
}
