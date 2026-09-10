using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class Scene3Quest : MonoBehaviour, IInteractable
{
    [Header("UI Elements")]
    public GameObject objectiveUI;          // I-drag dito ang Objective Panel / Canvas
    public TextMeshProUGUI objectiveText;   // I-drag dito ang ComputerObjectiveText Component

    [Header("Quest Targets & Doors")]
    [Header("Quest Targets")]
    public GameObject computerObjective;  // Dito mo i-drag ang Computer Objective
    public GameObject greenMesh;          // Dito mo i-drag ang Pickup (8) / Access Card

    [Header("Doors to Hide")]
    public GameObject door;               // Dito mo i-drag ang doors
    public GameObject object015;
    public GameObject object016;

    [Header("Transition Settings")]
    public GameObject sceneTransitionZone; 
    public string nextSceneName = "Scene4";

    private bool pcInteracted = false;

    private void Start()
    {
        // 1. Siguraduhing LITAW ang Objective UI at nakalagay ang panimulang Text
        if (objectiveUI != null) 
        {
            objectiveUI.SetActive(true);
        }

        if (objectiveText != null)
        {
            objectiveText.text = "Objective: Access the Computer";
        }

        // 2. Naka-OFF muna ang transition zone sa simula
        if (sceneTransitionZone != null)
        {
            sceneTransitionZone.SetActive(false);
        }
    }

    // Tatawagin kapag pinindot ang 'E' sa Computer Terminal
    public void Interact()
    {
        if (!pcInteracted)
        {
            pcInteracted = true;

            if (objectiveText != null)
            {
                objectiveText.text = "Objective: Find the access card!";
            }
        }
    }

    // Tatawagin kapag pinulot ang Access Card (Pickup 8)
    public void PickupGreenMesh()
    {
        // Matutunaw / Mawawala ang Card at ang mga Pinto
        HideObject(greenMesh);
        HideObject(door);
        HideObject(object015);
        HideObject(object016);

        // Bubukas ang Transition Zone para sa Scene 4
        if (sceneTransitionZone != null)
        {
            sceneTransitionZone.SetActive(true);
        }

        // Lalabas ang Door Unlocked Text
        if (objectiveText != null)
        {
            objectiveText.text = "Objective: Door Unlocked! Proceed to Scene 4.";
        }
    }

    // Helper function para siguradong mawala ang Visual Renderers at Colliders ng pinto
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