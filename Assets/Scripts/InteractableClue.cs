using UnityEngine;
using UnityEngine.UI;

public class InteractableClue : MonoBehaviour
{
    // Global variable para sa kasalukuyang clue na malapit sa player
    public static InteractableClue currentActiveClue;

    [Header("UI Panels & Buttons")]
    public GameObject cluePanelUI; // Tanging UI panel lang para sa clue na ito (hal. Clue1_PaperPanel)
    public GameObject promptUI;    // UI Text (hal. "Press G to Inspect")
    public Button inspectButtonG;  // Inspect Button (G) galing sa MobileControls

    [Header("Tutorial Link")]
    public TutorialManager tutorialManager;

    private bool isPlayerInRange = false;

    private void Start()
    {
        if (cluePanelUI != null) cluePanelUI.SetActive(false);
        if (promptUI != null) promptUI.SetActive(false);

        if (tutorialManager == null)
        {
            tutorialManager = FindFirstObjectByType<TutorialManager>();
        }
    }

    private void Update()
    {
        // Gagana LANG ang Keycode 'G' kapag ang CLUE NA ITO ang mismong malapit sa Player
        if (isPlayerInRange && currentActiveClue == this && Input.GetKeyDown(KeyCode.G))
        {
            ToggleCluePanel();
        }
    }

    public void ToggleCluePanel()
    {
        // Gagana LAMANG kapag nasa range at ito ang aktibong clue
        if (!isPlayerInRange || currentActiveClue != this || cluePanelUI == null) return;

        bool currentState = cluePanelUI.activeSelf;
        bool nextState = !currentState;

        cluePanelUI.SetActive(nextState);

        if (promptUI != null) promptUI.SetActive(!nextState);

        if (nextState && tutorialManager != null)
        {
            tutorialManager.CompleteInspectTask();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            currentActiveClue = this; // Set ito bilang ang natatanging aktibong clue!

            if (promptUI != null && (cluePanelUI == null || !cluePanelUI.activeSelf))
            {
                promptUI.SetActive(true);
            }

            if (inspectButtonG != null)
            {
                inspectButtonG.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            // Kapag umalis, alisin ito bilang active clue
            if (currentActiveClue == this)
            {
                currentActiveClue = null;
            }

            if (promptUI != null) promptUI.SetActive(false);
            if (cluePanelUI != null) cluePanelUI.SetActive(false);
            if (inspectButtonG != null) inspectButtonG.gameObject.SetActive(false);
        }
    }

    // Static helper function para sa Mobile Button On Click
    public static void TriggerCurrentClue()
    {
        if (currentActiveClue != null)
        {
            currentActiveClue.ToggleCluePanel();
        }
    }
}