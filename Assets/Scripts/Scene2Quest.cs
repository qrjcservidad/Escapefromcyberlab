using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class Scene2Quest : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject terminalUI;
    public TextMeshProUGUI terminalFeedbackText;
    public TMP_Text timerText;

    [Header("Intro & Level Title Display")]
    public GameObject levelTitleUI; // Slot for Level 2 UI/Text Object
    public TMP_Text storyText;       // Slot for Story Intro Text (TextMeshPro)
    public float introDisplayDuration = 8f; // Displays for 8 seconds before hiding

    [Header("Game Over Settings")]
    public GameObject gameOverUI; // Slot for Game Over UI Panel or Text Object (Optional)
    public TMP_Text gameOverText; // Slot specifically for Game Over TMP Text (Optional)

    [Header("Door Obstacles & Transition")]
    public GameObject object013; // Door barrier 1
    public GameObject object014; // Door barrier 2
    public GameObject sceneTransitionZone;

    [Header("Scene Settings")]
    public string nextSceneName = "Scene3"; // Name of Scene 3 in Build Settings

    [Header("Timer Settings")]
    public float timeRemaining = 90f; // 1 minute 30 seconds
    private bool isTimerRunning = true;
    private bool questCompleted = false;

    private Coroutine clearTextCoroutine;

    private void Start()
    {
        if (terminalUI != null) terminalUI.SetActive(false);
        if (terminalFeedbackText != null) terminalFeedbackText.text = "";

        if (gameOverUI != null) gameOverUI.SetActive(false);

        // Keep the transition zone disabled until the door is unlocked
        if (sceneTransitionZone != null)
        {
            sceneTransitionZone.SetActive(false);
        }

        // Set up and display story intro text
        if (storyText != null)
        {
            storyText.gameObject.SetActive(true);
            storyText.text = "you finish the first room and still you still found yourself in another room\n" +
                             "will this ever end?";
        }

        if (levelTitleUI != null)
        {
            levelTitleUI.SetActive(true);
        }

        // Start 8-second hide sequence for intro text and level title
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

    public void OnCorrectPassword()
    {
        if (questCompleted) return;

        questCompleted = true;
        isTimerRunning = false;

        if (terminalUI != null) terminalUI.SetActive(false);

        // Remove the door obstacles
        if (object013 != null) object013.SetActive(false);
        if (object014 != null) object014.SetActive(false);

        // Enable transition zone
        if (sceneTransitionZone != null)
        {
            sceneTransitionZone.SetActive(true);
        }
    }

    // =========================================================
    // PAG MALI ANG SAGOT / CODE: BAWAS 20 SECONDS SA TIMER
    // =========================================================
    public void OnWrongPassword()
    {
        if (questCompleted || !isTimerRunning) return;

        timeRemaining -= 20f;
        if (timeRemaining < 0) timeRemaining = 0;

        if (terminalFeedbackText != null)
        {
            terminalFeedbackText.color = Color.red;
            terminalFeedbackText.text = "ERROR: INCORRECT ANSWER!\n-20 SECONDS PENALTY!";

            if (clearTextCoroutine != null)
            {
                StopCoroutine(clearTextCoroutine);
            }

            clearTextCoroutine = StartCoroutine(ClearFeedbackTextAfterDelay(2.0f));
        }

        if (timeRemaining <= 0)
        {
            GameOver();
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
        if (terminalUI != null) terminalUI.SetActive(false);

        if (gameOverUI != null) gameOverUI.SetActive(true);

        if (gameOverText != null)
        {
            gameOverText.color = Color.red;
            gameOverText.text = "GAME OVER";
        }

        if (timerText != null)
        {
            timerText.color = Color.red;
            timerText.text = "GAME OVER";
        }

        Invoke(nameof(RestartScene), 2f);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}