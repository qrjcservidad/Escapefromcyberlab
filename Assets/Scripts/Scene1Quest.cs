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

    [Header("Intro & Level Title Display")]
    public GameObject levelTitleUI; // Slot para sa Level 1 UI/Text Object
    public TMP_Text storyText;       // Slot para sa Story Intro Text (TextMeshPro)
    public float introDisplayDuration = 8f; // Ilang segundo bago mawala (8 seconds)

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

        // Set ang story text content
        if (storyText != null)
        {
            storyText.gameObject.SetActive(true);
            storyText.text = "you woke up in this unknown place\n" +
                             "you scan your surroundings and you find no one\n" +
                             "you try and shout to get anyone's attention to no avail\n" +
                             "i guess the only option now is to escape";
        }

        if (levelTitleUI != null)
        {
            levelTitleUI.SetActive(true);
        }

        // Simulan ang 6-second hide sequence para sa Intro at Level Title
        StartCoroutine(HideIntroRoutine());
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

    private IEnumerator HideIntroRoutine()
    {
        yield return new WaitForSeconds(introDisplayDuration);

        // Itatago ang level title at story text pagkaraan ng 6 seconds
        if (levelTitleUI != null)
        {
            levelTitleUI.SetActive(false);
        }

        if (storyText != null)
        {
            storyText.gameObject.SetActive(false);
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

    // =========================================================
    // TAWAGIN ITO SA ON CLICK () EVENT NG "correct horse battery staple" BUTTON
    // =========================================================
    public void OnCorrectAnswer()
    {
        questCompleted = true;
        isTimerRunning = false;

        if (terminalFeedbackText != null)
        {
            terminalFeedbackText.color = Color.green;
            terminalFeedbackText.text = "DOOR UNLOCKED!\nYOU CAN PROCEED TO THE DOOR!";
        }

        StartCoroutine(CompleteQuestSequenceAfterDelay(2.0f));
    }

    // =========================================================
    // TAWAGIN ITO SA ON CLICK () EVENT NG MGA MALING SAGOT NA BUTTON
    // =========================================================
    public void OnWrongAnswer()
    {
        OnWrongPassword();
    }

    private IEnumerator CompleteQuestSequenceAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

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

    public void OnCorrectPassword()
    {
        OnCorrectAnswer();
    }

    // =========================================================
    // PAG MALI ANG PASSWORD O SELECTION: BAWAS 20 SECONDS SA TIMER
    // =========================================================
    public void OnWrongPassword()
    {
        // Babawasan ng 20 seconds ang natitirang oras
        timeRemaining -= 20f;
        if (timeRemaining < 0) timeRemaining = 0;

        if (terminalFeedbackText != null)
        {
            terminalFeedbackText.color = Color.red;
            terminalFeedbackText.text = "ERROR: INCORRECT PASSWORD!\n-20 SECONDS PENALTY!";

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
            timerText.text = "<color=red><b><size=160%>TIME'S UP!</size></b></color>";
        }

        Invoke(nameof(RestartScene), 2f);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}