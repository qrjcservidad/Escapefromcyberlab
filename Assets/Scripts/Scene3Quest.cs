using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class Scene3Quest : MonoBehaviour, IInteractable
{
    [Header("Intro & Level Title Display")]
    public GameObject levelTitleUI; // Slot para sa Level 3 UI/Text Object
    public TMP_Text storyText;       // Slot para sa Story Intro Text (TextMeshPro)
    public float introDisplayDuration = 8f; // Mawawala pagkaraan ng 8 seconds

    [Header("Quest Targets & Doors")]
    public GameObject computerObjective;  
    public GameObject greenMesh;           // Access Card

    [Header("Doors to Hide")]
    public GameObject door;                // doors container
    public GameObject object015;           // Object015 (pinto 1)
    public GameObject object016;           // Object016 (pinto 2)

    [Header("Transition Settings")]
    public GameObject sceneTransitionZone; 
    public string nextSceneName = "Scene4";

    [Header("Timer Settings")]
    public TMP_Text timerText;             // Drag TimerText UI dito
    public float timeRemaining = 120f;     // Total seconds (e.g. 120s = 2 minutes)
    private bool isTimerRunning = true;

    private bool pcInteracted = false;

    private void Start()
    {
        if (sceneTransitionZone != null)
        {
            sceneTransitionZone.SetActive(false);
        }

        // Siguraduhing buhay at na-update agad ang Timer UI sa unang frame
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
            UpdateTimerUI(timeRemaining);
        }
        else
        {
            Debug.LogError("Scene3Quest: WALANG NAKA-ASSIGN NA TIMERTEXT SA INSPECTOR! Paki-drag ang TimerText UI.");
        }

        // Set up ng Story Text Content para sa Scene 3
        if (storyText != null)
        {
            storyText.gameObject.SetActive(true);
            storyText.text = "you feel the end is drawing near\n" +
                             "you musn't lose hope";
        }

        if (levelTitleUI != null)
        {
            levelTitleUI.SetActive(true);
        }

        // Simulan ang 8-second hide routine para sa Intro Text at Level Title
        StartCoroutine(HideIntroRoutine());
    }

    private void Update()
    {
        // Countdown timer process
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                isTimerRunning = false;
                UpdateTimerUI(timeRemaining);
                OnTimeExpired();
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

    // Helper para sa formatting ng Timer UI (MM:SS)
    private void UpdateTimerUI(float timeToDisplay)
    {
        if (timerText == null) return;

        if (timeToDisplay < 0) timeToDisplay = 0;

        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Tatawagin kapag nagkamali sa quiz para mabawasan ang oras
    public void DeductTime(float penaltySeconds)
    {
        timeRemaining -= penaltySeconds;
        if (timeRemaining < 0) timeRemaining = 0;
        UpdateTimerUI(timeRemaining); // Agad na ire-refresh ang UI Text
        Debug.Log($"Bawas {penaltySeconds}s sa oras! Remaining: {timeRemaining}");
    }

    // Kapag naubos ang oras (Game Over / Restart Scene)
    private void OnTimeExpired()
    {
        Debug.Log("Time Expired! Restarting Scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Interact()
    {
        if (!pcInteracted)
        {
            pcInteracted = true;
        }
    }

    public void PickupGreenMesh()
    {
        HideObject(greenMesh);
    }

    // TATAWAGIN ITO KAPAG TAMA LAHAT ANG SAGOT SA QUIZ
    public void OpenDoors()
    {
        isTimerRunning = false; // Ititigil ang timer kapag nanalo
        
        HideObject(door);
        HideObject(object015);
        HideObject(object016);

        // Bubukas ang Transition Zone para sa Scene 4
        if (sceneTransitionZone != null)
        {
            sceneTransitionZone.SetActive(true);
        }

        Debug.Log("Scene3Quest: Natapos ang Quiz! Nasira/Nawala na ang mga pinto at nabuksan ang Transition Zone.");
    }

    private void HideObject(GameObject target)
    {
        if (target == null) return;

        Renderer[] renderers = target.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }

        Collider[] colliders = target.GetComponentsInChildren<Collider>(true);
        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }

        target.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}