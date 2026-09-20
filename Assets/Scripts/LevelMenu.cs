using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    // Para sa Level Buttons 1, 2, 3, at 4 (May space)
    public void OpenLevel(int levelId)
    {
        // Lalabas na "Scene 1", "Scene 2", "Scene 3", "Scene 4"
        string levelName = "Scene " + levelId; 
        SceneManager.LoadScene(levelName);
    }

    // Para sa User Guide / Tutorial Button
    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}