using UnityEngine;

public class InteractableClue : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject cluePanelUI; // Popup panel para sa clue na ito
    public GameObject promptUI;    // UI Text (e.g., "Press G to Inspect")

    [Header("Tutorial Link")]
    public TutorialManager tutorialManager; // Drag the GameObject with TutorialManager here

    private bool isPlayerInRange = false;

    private void Start()
    {
        if (cluePanelUI != null) cluePanelUI.SetActive(false);
        if (promptUI != null) promptUI.SetActive(false);

        // Backup plan: Pag nakalimutan mong i-drag, hahanapin pa rin nito sa scene
        if (tutorialManager == null)
        {
            tutorialManager = FindFirstObjectByType<TutorialManager>();
        }
    }

    private void Update()
    {
        // Toggle UI display kapag pinindot ang 'G' key
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.G))
        {
            if (cluePanelUI != null)
            {
                bool currentState = cluePanelUI.activeSelf;
                bool nextState = !currentState;

                cluePanelUI.SetActive(nextState);

                // Itago ang "Press G" text kapag bukas ang clue UI
                if (promptUI != null) promptUI.SetActive(!nextState);

                // Magche-check LANG kapag BINUKSAN ang board
                if (nextState && tutorialManager != null)
                {
                    tutorialManager.CompleteInspectTask();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (promptUI != null && (cluePanelUI == null || !cluePanelUI.activeSelf))
            {
                promptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (promptUI != null) promptUI.SetActive(false);
            if (cluePanelUI != null) cluePanelUI.SetActive(false);
        }
    }
}