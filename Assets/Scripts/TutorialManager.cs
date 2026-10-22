using UnityEngine;
using TMPro; // Kailangan para sa TextMeshPro UI

public class TutorialManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TMP_Text tutorialText;

    [Header("Scene Transition & Doors")]
    public GameObject sceneTransitionZone;
    public GameObject object009; // Slot para sa Object009
    public GameObject object010; // Slot para sa Object010

    [Header("Objectives Tracking")]
    private bool movedWASD = false;
    private bool ranShift = false;
    private bool inspectedItem = false;
    private bool usedTerminal = false;
    private bool pickedUpCard = false;

    private void Start()
    {
        // I-hide muna ang scene transition zone at pinto sa simula
        if (sceneTransitionZone != null) sceneTransitionZone.SetActive(false);
        if (object009 != null) object009.SetActive(true); // Nakasara ang pinto/harang
        if (object010 != null) object010.SetActive(true);

        UpdateTutorialUI();
    }

    private void Update()
    {
        // Auto-detect movement galing sa Keyboard, Mobile Touch, o On-Screen Joystick
        if (!movedWASD)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Direct check para sa New Input System Gamepad/On-Screen Stick o Legacy Input
            bool isJoystickMoved = false;
            #if ENABLE_INPUT_SYSTEM
            if (UnityEngine.InputSystem.Gamepad.current != null)
            {
                Vector2 stick = UnityEngine.InputSystem.Gamepad.current.leftStick.ReadValue();
                if (stick.magnitude > 0.1f) isJoystickMoved = true;
            }
            #endif

            // Check kung may touch drag sa screen o keyboard/joystick movement
            if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f || isJoystickMoved || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
            {
                CompleteMoveTask();
            }
        }
    }

    // --- CALLABLE PUBLIC FUNCTIONS PARA SA MGA BUTTONS AT SCRIPTS ---

    public void CompleteMoveTask()
    {
        if (!movedWASD)
        {
            movedWASD = true;
            UpdateTutorialUI();
        }
    }

    // TAWAGAN SA SPRINT BUTTON ON-CLICK O PLAYER MOVEMENT SCRIPT
    public void CompleteSprintTask()
    {
        if (movedWASD && !ranShift)
        {
            ranShift = true;
            UpdateTutorialUI();
        }
    }

    // TAWAGAN SA INSPECT BUTTON (G) SCRIPT
    public void CompleteInspectTask()
    {
        if (ranShift && !inspectedItem)
        {
            inspectedItem = true;
            UpdateTutorialUI();
        }
    }

    // TAWAGAN SA COMPUTER INTERACTABLE SCRIPT (OpenComputerUI)
    public void CompleteTerminalTask()
    {
        if (inspectedItem && !usedTerminal)
        {
            usedTerminal = true;
            UpdateTutorialUI();
        }
    }

    // TAWAGAN SA PICKUP CARD SCRIPT
    public void CompletePickupCardTask()
    {
        if (usedTerminal && !pickedUpCard)
        {
            pickedUpCard = true;
            UpdateTutorialUI();
            UnlockExitDoor();
        }
    }

    // BACKUP ALIAS: Para maiwasan ang CS1061 error kung CompletePickupTask ang tinawag
    public void CompletePickupTask()
    {
        CompletePickupCardTask();
    }

    // --- UI DISPLAY AT PROGRESSION LOGIC ---

    private void UpdateTutorialUI()
    {
        if (tutorialText == null) return;

        if (!movedWASD)
        {
            tutorialText.text = "Tutorial: Use the Joystick to Move";
        }
        else if (!ranShift)
        {
            tutorialText.text = "Tutorial: Press the Sprint Icon to Run";
        }
        else if (!inspectedItem)
        {
            tutorialText.text = "Tutorial: Approach an Object and Press [G] to Inspect";
        }
        else if (!usedTerminal)
        {
            tutorialText.text = "Tutorial: Approach the Computer and Press [E] to Access Terminal";
        }
        else if (!pickedUpCard)
        {
            tutorialText.text = "Tutorial: Press [F] to Pickup the Access Card";
        }
        else
        {
            tutorialText.text = "Tutorial Complete! Proceed to the Exit Door.";
        }
    }

    private void UnlockExitDoor()
    {
        // Buksan ang pinto / Enable ang scene transition zone kapag tapos na ang lahat ng tasks
        if (sceneTransitionZone != null) sceneTransitionZone.SetActive(true);
        if (object009 != null) object009.SetActive(false);
        if (object010 != null) object010.SetActive(false);
    }
}