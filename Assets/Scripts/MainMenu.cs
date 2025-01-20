using UnityEngine;
using UnityEngine.SceneManagement;  // To handle scene loading
using UnityEngine.UI;              // To interact with UI buttons

public class MainMenu : MonoBehaviour
{
    // You can link these buttons and images via the Inspector
    public Button startButton;
    public Button quitButton;
    public Button settingsButton;  // The new button for opening settings
    public Button backButton;     // The new button for going back to the main menu

    public GameObject mainMenuImage;  // The main menu image (to disable)
    public GameObject settingsImage;  // The settings image (to enable)

    void Start()
    {
        // Assign listeners to the buttons
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

        if (backButton != null)
        {
            backButton.onClick.AddListener(CloseSettings);
        }
    }

    // Load the next scene (assuming it's added to your build settings)
    void LoadNextScene()
    {
        // Change the scene by its name or index.
        // Ensure the next scene is added in your build settings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Quit the game
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

    // Open the settings (disable main menu image and enable settings image)
    void OpenSettings()
    {
        if (mainMenuImage != null)
        {
            mainMenuImage.SetActive(false);  // Disable the main menu image
        }

        if (settingsImage != null)
        {
            settingsImage.SetActive(true);  // Enable the settings image
        }
    }

    // Close the settings (enable the main menu image and disable settings image)
    void CloseSettings()
    {
        if (settingsImage != null)
        {
            settingsImage.SetActive(false);  // Disable the settings image
        }

        if (mainMenuImage != null)
        {
            mainMenuImage.SetActive(true);  // Enable the main menu image
        }
    }
}
