using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AudioUIManager : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider ambienceSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        sfxSlider.onValueChanged.AddListener(HandleSFXChanged);
        ambienceSlider.onValueChanged.AddListener(HandleAmbienceChanged);
        musicSlider.onValueChanged.AddListener(HandleMusicChanged);

        sfxSlider.value = AudioManager.Instance.SFX_Volume;
        ambienceSlider.value = AudioManager.Instance.AmbienceVolume;
        musicSlider.value = AudioManager.Instance.MusicVolume;
    }

    private void HandleSFXChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }

    private void HandleAmbienceChanged(float value)
    {
        AudioManager.Instance.SetAmbienceVolume(value);
    }

    private void HandleMusicChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    private void OnDestroy()
    {
        sfxSlider.onValueChanged.RemoveListener(HandleSFXChanged);
        ambienceSlider.onValueChanged.RemoveListener(HandleAmbienceChanged);
        musicSlider.onValueChanged.RemoveListener(HandleMusicChanged);
    }
}
