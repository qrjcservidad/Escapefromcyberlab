using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class Scene1Quest : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject terminalUI;
    public TMP_Text objectiveText;
    public TMP_Text terminalFeedbackText; 

    [Header("Door & Transition Settings")]
    public GameObject door;
    public GameObject object011;
    public GameObject object012;
    public GameObject sceneTransitionZone; // Idinagdag ang slot para sa trigger zone

    private Coroutine clearTextCoroutine;

    private void Start()
    {
        if (terminalUI != null) terminalUI.SetActive(false);

        if (objectiveText != null)
        {
            objectiveText.text = "Objective: Access the Terminal";
        }
        
        if (terminalFeedbackText != null) terminalFeedbackText.text = ""; 

        // Siguraduhing NAKA-OFF muna ang transition zone sa simula
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
        
        // Alisin ang pinto at iba pang harang
        if (door != null) door.SetActive(false);
        if (object011 != null) object011.SetActive(false);
        if (object012 != null) object012.SetActive(false);

        // BUKAS NA ANG PINTO: Tsaka lang i-eenable ang Scene Transition Trigger Zone!
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
            objectiveText.text = "Access Denied! Check whiteboard for hint.";
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
}