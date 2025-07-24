using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    public float SFX_Volume { get; private set; } = 1f;
    public float AmbienceVolume { get; private set; } = 0.5f;
    public float MusicVolume { get; private set; } = 0.25f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetSFXVolume(SFX_Volume);
        SetAmbienceVolume(AmbienceVolume);
        SetMusicVolume(MusicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume("SFXVolume", volume);
        SFX_Volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        SetVolume("MusicVolume", volume);
        MusicVolume = volume;
    }

    public void SetAmbienceVolume(float volume)
    {
        SetVolume("AmbienceVolume", volume);
        AmbienceVolume = volume;
    }

    private void SetVolume(string exposedParam, float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, dB);
    }
}
