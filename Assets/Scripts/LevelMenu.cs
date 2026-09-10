using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public void OpenLevel(int levelId)
    {
        // appear Scene 1 2 3 and 4
        string levelName = "Scene " + levelId; 
        SceneManager.LoadScene(levelName);
    }
}