using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Quit Confirmation UI")]
    [SerializeField] private GameObject quitConfirmationPanel;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    // Tawagin ito kapag pinindot ang main "EXIT" button para lumabas ang confirmation panel
    public void OpenQuitConfirmation()
    {
        if (quitConfirmationPanel != null)
        {
            quitConfirmationPanel.SetActive(true);
        }
    }

    // Tawagin ito kapag pinindot ang "NO" button para isara ang confirmation panel
    public void CloseQuitConfirmation()
    {
        if (quitConfirmationPanel != null)
        {
            quitConfirmationPanel.SetActive(false);
        }
    }

    // Tawagin ito kapag pinindot ang "YES" button para tuluyang mag-exit
    public void ConfirmQuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Gagana ito habang nag-e-test sa Unity Editor
        #else
            Application.Quit(); // Gagana ito kapag na-build na ang game (Android/PC)
        #endif
    }
}