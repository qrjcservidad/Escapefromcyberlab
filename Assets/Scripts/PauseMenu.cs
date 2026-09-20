using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuUI;    // Panel ng Pause Menu
    public GameObject optionsMenuUI;  // Panel ng Options/Settings

    public static bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // Kung nakabukas ang options menu, bumalik muna sa pause menu
                if (optionsMenuUI != null && optionsMenuUI.activeSelf)
                {
                    BackToPauseMenu();
                }
                else
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (optionsMenuUI != null) optionsMenuUI.SetActive(false);

        Time.timeScale = 1f; // Ipagpatuloy ang laro
        isPaused = false;

        // I-lock at itago ang cursor para sa camera control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        if (optionsMenuUI != null) optionsMenuUI.SetActive(false);

        Time.timeScale = 0f; // I-pause ang laro
        isPaused = true;

        // I-unlock at ipakita ang cursor para makapindot sa menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // TAWAGAN ITO SA SETTINGS BUTTON (OpenSettings)
    public void OpenSettings()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);   // Itatago ang Pause Menu
        if (optionsMenuUI != null) optionsMenuUI.SetActive(true); // Lalabas ang Options Panel
    }

    // TAWAGAN ITO SA BACK BUTTON SA LOOB NG OPTIONS PANEL
    public void BackToPauseMenu()
    {
        if (optionsMenuUI != null) optionsMenuUI.SetActive(false); // Itatago ang Options Panel
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);    // Lalabas muli ang Pause Menu
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu"); 
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}