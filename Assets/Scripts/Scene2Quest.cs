using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class Scene2Quest : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject terminalUI;
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI terminalFeedbackText;

    [Header("Door Obstacles & Transition")]
    public GameObject object013; // Door barrier 1
    public GameObject object014; // Door barrier 2
    public GameObject sceneTransitionZone;

    [Header("Scene Settings")]
    public string nextSceneName = "Scene3"; // Name of Scene 3 in Build Settings

    private Coroutine clearTextCoroutine;

    private void Start()
    {
        if (terminalUI != null) terminalUI.SetActive(false);

        if (objectiveText != null)
        {
            objectiveText.text = "Objective: Access the Terminal";
        }

        if (terminalFeedbackText != null) terminalFeedbackText.text = "";

        // Keep the transition zone disabled until the door is unlocked
        if (sceneTransitionZone != null)
        {
            sceneTransitionZone.SetActive(false);
        }
    }

    public void OpenTerminal()
    {
        if (terminalUI != null) terminalUI.SetActive(true);
        if (terminalFeedbackText != null) terminalFeedbackText.text = "";
    }

    public void OnCorrectPassword()
    {
        if (terminalUI != null) terminalUI.SetActive(false);

        // Remove the door obstacles (object013 & object014)
        if (object013 != null) object013.SetActive(false);
        if (object014 != null) object014.SetActive(false);

        // Enable the Scene Transition Trigger Zone in the doorway
        if (sceneTransitionZone != null)
        {
            sceneTransitionZone.SetActive(true);
        }

        if (objectiveText != null)
        {
            objectiveText.text = "Objective: Door Unlocked! Proceed through the doorway.";
        }
    }

    public void OnWrongPassword()
    {
        if (terminalFeedbackText != null)
        {
            terminalFeedbackText.color = Color.red;
            terminalFeedbackText.text = "ERROR: INCORRECT PASSWORD!\nACCESS DENIED.";

            if (clearTextCoroutine != null)
            {
                StopCoroutine(clearTextCoroutine);
            }

            clearTextCoroutine = StartCoroutine(ClearFeedbackTextAfterDelay(2.0f));
        }

        if (objectiveText != null)
        {
            objectiveText.text = "Access Denied! Check hints around Scene 2.";
        }
    }

    private IEnumerator ClearFeedbackTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (terminalFeedbackText != null)
        {
            terminalFeedbackText.text = "";
        }
    }

    // Call this via OnTriggerEnter on the sceneTransitionZone or directly on trigger collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}