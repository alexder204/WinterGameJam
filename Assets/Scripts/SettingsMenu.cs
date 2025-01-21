using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // References to the UI sliders and buttons in the Settings scene
    public Slider musicVolumeSlider;  // Slider for adjusting music volume
    public Slider sfxVolumeSlider;    // Slider for adjusting SFX volume
    public Button controlsButton;     // Button to go to controls menu
    public Button backButton;         // Button to go back to settings menu

    // Reference to the AudioManager
    private AudioManager audioManager;

    // GameObject references for the UI windows
    public GameObject settingsMenu;   // The settings menu
    public GameObject controlsMenu;   // The controls menu

    void Start()
    {
        // Get the AudioManager instance (make sure it exists)
        audioManager = AudioManager.instance;

        // Set the initial slider values based on the current audio settings
        if (audioManager != null)
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        }

        // Set up listeners for the sliders
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        // Set up listeners for the buttons
        if (controlsButton != null)
        {
            controlsButton.onClick.AddListener(OpenControlsMenu);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(BackToSettingsMenu);
        }
    }

    // Set the music volume
    public void SetMusicVolume(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(volume);  // Set music volume in AudioManager
        }
    }

    // Set the SFX volume
    public void SetSFXVolume(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetSFXVolume(volume);  // Set SFX volume in AudioManager
        }
    }

    // Open the controls menu (hide settings and show controls)
    public void OpenControlsMenu()
    {
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(false);  // Disable settings menu
        }

        if (controlsMenu != null)
        {
            controlsMenu.SetActive(true);   // Enable controls menu
        }
    }

    // Back to the settings menu from the controls menu
    public void BackToSettingsMenu()
    {
        if (controlsMenu != null)
        {
            controlsMenu.SetActive(false);  // Disable controls menu
        }

        if (settingsMenu != null)
        {
            settingsMenu.SetActive(true);   // Enable settings menu
        }
    }
}
