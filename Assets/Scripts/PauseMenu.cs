using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;
using TMPro;  

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject settingsMenu;
    public GameObject controlsMenu;
    [SerializeField] public Slider healthSlider;
    [SerializeField] public TextMeshProUGUI scoreText;

    public static bool isActive = false;

    // References to the buttons
    [SerializeField] private Button quitButton;
    [SerializeField] private Button quitToSceneButton;
    public Button settingsButton;
    public Button settingsBackButton;
    public Button controlsButton;
    public Button controlsBackButton;

    public string sceneToLoad;

    void Start()
    {
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }

        if (quitToSceneButton != null)
        {
            quitToSceneButton.onClick.AddListener(QuitToScene);
        }

        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OpenSettings);
        }

        if (settingsBackButton != null)
        {
            settingsBackButton.onClick.AddListener(CloseSettings);
        }
        if (controlsButton != null)
        {
            controlsButton.onClick.AddListener(OpenControlsMenu);
        }

        if (controlsBackButton != null)
        {
            controlsBackButton.onClick.AddListener(BackToSettingsMenu);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isActive)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isActive = false;

        // Enable health slider and score text when resuming the game
        if (healthSlider != null) healthSlider.gameObject.SetActive(true);
        if (scoreText != null) scoreText.gameObject.SetActive(true);
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isActive = true;

        // Disable health slider and score text when pausing the game
        if (healthSlider != null) healthSlider.gameObject.SetActive(false);
        if (scoreText != null) scoreText.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        // If the game is running in the editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // For a built game, quit the application
        Application.Quit();
#endif
    }

    public void QuitToScene()
    {
        Time.timeScale = 1f;
        isActive = false;

        // Enable health slider and score text when resuming the game
        if (healthSlider != null && !healthSlider.gameObject.activeSelf)
        {
            healthSlider.gameObject.SetActive(true);
        }

        if (scoreText != null && !scoreText.gameObject.activeSelf)
        {
            scoreText.gameObject.SetActive(true);
        }

        // Check if the scene name is valid
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            // Load the specified scene
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("Scene name is not specified!");
        }
    }

    void OpenSettings()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        if (settingsMenu != null)
        {
            settingsMenu.SetActive(true);
        }
    }

    void CloseSettings()
    {
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }
    }

    public void OpenControlsMenu()
    {
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(false);
        }

        if (controlsMenu != null)
        {
            controlsMenu.SetActive(true);
        }
    }

    public void BackToSettingsMenu()
    {
        if (controlsMenu != null)
        {
            controlsMenu.SetActive(false);
        }

        if (settingsMenu != null)
        {
            settingsMenu.SetActive(true);
        }
    }
}
