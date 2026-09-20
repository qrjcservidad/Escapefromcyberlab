using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class Scene1Quest : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject terminalUI;
    public TMP_Text terminalFeedbackText; 
    public TMP_Text timerText;

    [Header("Level Title Display")]
    public GameObject levelTitleUI; // Slot para sa Level 1 UI/Text Object
    public float levelTitleDuration = 6f; // Ilang segundo bago mawala (6 seconds)

    [Header("Door & Transition Settings")]
    public GameObject door;
    public GameObject object011;
    public GameObject object012;
    public GameObject sceneTransitionZone;

    [Header("Timer Settings")]
    public float timeRemaining = 90f; // 1 minute 30 seconds
    private bool isTimerRunning = true;
    private bool questCompleted = false;

    private Coroutine clearTextCoroutine;

    private void Start()
    {
        if (terminalUI != null) terminalUI.SetActive(false);
        if (terminalFeedbackText != null) terminalFeedbackText.text = ""; 

        if (sceneTransitionZone != null)
        {
            sceneTransitionZone.SetActive(false);
        }

        // Simulan ang Level Title 6-second hide sequence
        if (levelTitleUI != null)
        {
            levelTitleUI.SetActive(true);
            StartCoroutine(HideLevelTitleRoutine());
        }
    }

    private void Update()
    {
        // Countdown Timer Logic
        if (isTimerRunning && !questCompleted)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                isTimerRunning = false;
                GameOver();
            }
        }
    }

    private IEnumerator HideLevelTitleRoutine()
    {
        yield return new WaitForSeconds(levelTitleDuration);

        if (levelTitleUI != null)
        {
            levelTitleUI.SetActive(false);
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        if (timerText == null) return;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("Time Left: {0:00}:{1:00}", minutes, seconds);

        if (timeToDisplay <= 15f)
        {
            timerText.color = Color.red;
        }
    }

    public void OpenTerminal()
    {
        if (terminalUI != null) terminalUI.SetActive(true);
        if (terminalFeedbackText != null) terminalFeedbackText.text = "";
    }

    public void OnCorrectPassword()
    {
        questCompleted = true;
        isTimerRunning = false;

        if (terminalUI != null) terminalUI.SetActive(false);

        // Alisin ang pinto at buksan ang transition zone
        if (door != null) door.SetActive(false);
        if (object011 != null) object011.SetActive(false);
        if (object012 != null) object012.SetActive(false);

        if (sceneTransitionZone != null)
        {
            sceneTransitionZone.SetActive(true);
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
    }

    private IEnumerator ClearFeedbackTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (terminalFeedbackText != null)
        {
            terminalFeedbackText.text = "";
        }
    }

    private void GameOver()
    {
        if (timerText != null)
        {
            timerText.text = "<color=red><b>TIME'S UP!</b></color>";
        }

        Invoke(nameof(RestartScene), 2f);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}