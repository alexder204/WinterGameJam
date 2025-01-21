using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;              

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public Button settingsButton;
    public Button settingsBackButton;     
    public Button controlsButton;
    public Button controlsBackButton;

    public GameObject mainMenuImage;  
    public GameObject settingsMenu;   
    public GameObject controlsMenu;   

    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(LoadNextScene);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
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

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void QuitGame()
    {
        // If the game is running in the editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // For a built game, quit the application
        Application.Quit();
#endif
    }

    void OpenSettings()
    {
        if (mainMenuImage != null)
        {
            mainMenuImage.SetActive(false);
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

        if (mainMenuImage != null)
        {
            mainMenuImage.SetActive(true);
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
