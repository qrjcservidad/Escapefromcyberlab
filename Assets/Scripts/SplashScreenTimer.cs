using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenTimer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        // Pinalitan sa 1.5 seconds (siguraduhing may 'f' sa dulo)
        yield return new WaitForSeconds(1.5f); 
        
        SceneManager.LoadScene("Main Menu");
    }
}