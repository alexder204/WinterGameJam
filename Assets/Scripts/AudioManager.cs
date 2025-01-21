using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer audioMixer;
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;

    // Sliders for volume adjustment
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Load saved audio settings on start
        LoadAudioSettings();

        // Set sliders to the saved volume values
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);  // Default to 1 if not set
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);  // Default to 1 if not set
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);  // Save the volume setting
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXAudio", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);  // Save the volume setting
    }

    private void LoadAudioSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);  // Default to 1 if not set
        float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);      // Default to 1 if not set

        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }
}
