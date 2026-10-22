using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuUI;    // Panel ng Pause Menu
    public GameObject optionsMenuUI;  // Panel ng Options/Settings

    public static bool isPaused = false;

    void Start()
    {
        // Siguraduhing nakatago muna ang panels at tumatakbo ang oras
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (optionsMenuUI != null) optionsMenuUI.SetActive(false);
    }

    void Update()
    {
        // Pwedeng gamitin ang Escape key sa PC o Back button sa Android
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
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

    // TAWAGAN ITO SA PAUSE BUTTON SA SCREEN (Mobile Touch)
    public void OpenPauseMenu()
    {
        Pause();
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (optionsMenuUI != null) optionsMenuUI.SetActive(false);

        Time.timeScale = 1f; // Ipagpatuloy ang laro
        isPaused = false;
    }

    public void Pause()
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);

            // Sisiguraduhing nasa 1 ang scale at nasa gitna kapag nag-open
            RectTransform rect = pauseMenuUI.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.localScale = Vector3.one;
            }
        }

        if (optionsMenuUI != null) optionsMenuUI.SetActive(false);

        Time.timeScale = 0f; // I-pause ang laro
        isPaused = true;
    }

    // TAWAGAN ITO SA SETTINGS BUTTON
    public void OpenSettings()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (optionsMenuUI != null) optionsMenuUI.SetActive(true);
    }

    // TAWAGAN ITO SA BACK BUTTON SA OPTIONS PANEL
    public void BackToPauseMenu()
    {
        if (optionsMenuUI != null) optionsMenuUI.SetActive(false);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("Main Menu"); 
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}