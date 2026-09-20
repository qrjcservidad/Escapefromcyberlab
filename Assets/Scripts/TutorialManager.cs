using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text tutorialText;

    [Header("Scene Transition & Doors")]
    public GameObject sceneTransitionZone;
    public GameObject object009; // Slot para sa Object009
    public GameObject object010; // Slot para sa Object010

    // Objectives Tracking
    private bool movedWASD = false;
    private bool ranShift = false;
    private bool inspectedItem = false;
    private bool usedTerminal = false;
    private bool pickedUpCard = false;

    private void Start()
    {
        if (sceneTransitionZone != null)
        {
            sceneTransitionZone.SetActive(false);
        }

        UpdateTutorialUI();
    }

    private void Update()
    {
        // Check WASD Movement
        if (!movedWASD)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || 
                Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                movedWASD = true;
                UpdateTutorialUI();
            }
        }

        // Check Left Shift (Run)
        if (!ranShift)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ranShift = true;
                UpdateTutorialUI();
            }
        }
    }

    // Isasagawa lang kapag binuksan ang board/clue UI
    public void CompleteInspectTask()
    {
        if (!inspectedItem)
        {
            inspectedItem = true;
            UpdateTutorialUI();
        }
    }

    public void CompleteTerminalTask()
    {
        usedTerminal = true;
        UpdateTutorialUI();
    }

    public void CompletePickupTask()
    {
        pickedUpCard = true;
        UpdateTutorialUI();
    }

    private void UpdateTutorialUI()
    {
        if (tutorialText == null) return;

        string wasdCheck = movedWASD ? "<color=green>[✓]</color>" : "[ ]";
        string shiftCheck = ranShift ? "<color=green>[✓]</color>" : "[ ]";
        string inspectCheck = inspectedItem ? "<color=green>[✓]</color>" : "[ ]";
        string terminalCheck = usedTerminal ? "<color=green>[✓]</color>" : "[ ]";
        string cardCheck = pickedUpCard ? "<color=green>[✓]</color>" : "[ ]";

        tutorialText.text = $"<b>[ TUTORIAL OBJECTIVES ]</b>\n\n" +
                            $"{wasdCheck} Move: <b>WASD</b>\n" +
                            $"{shiftCheck} Run: <b>Left Shift</b>\n" +
                            $"{inspectCheck} Inspect: <b>G</b>\n" +
                            $"{terminalCheck} Access Terminal: <b>E</b>\n" +
                            $"{cardCheck} Pick up Keycard: <b>F</b>";

        // Pag matapos ang 5 tasks, ibubukas ang exit
        if (movedWASD && ranShift && inspectedItem && usedTerminal && pickedUpCard)
        {
            if (sceneTransitionZone != null)
            {
                sceneTransitionZone.SetActive(true);
            }

            if (object009 != null)
            {
                object009.SetActive(false);
            }

            if (object010 != null)
            {
                object010.SetActive(false);
            }

            tutorialText.text += "\n\n<color=yellow><b>Objective Complete! Head to the exit doors.</b></color>";
        }
    }
}