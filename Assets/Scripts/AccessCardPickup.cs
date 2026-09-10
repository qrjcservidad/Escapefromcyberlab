using UnityEngine;
using TMPro;

public class AccessCardPickup : MonoBehaviour
{
    [Header("Drag Objects Here")]
    public GameObject promptText;      // I-drag dito ang InteractPromptText
    public Transform playerTransform;  // I-drag dito ang Player GameObject

    [Header("Settings")]
    public float interactDistance = 4f;

    private RectTransform promptRect;
    private TMP_Text promptTextMesh;

    private void Start()
    {
        if (promptText != null)
        {
            promptRect = promptText.GetComponent<RectTransform>();
            promptTextMesh = promptText.GetComponent<TMP_Text>();
            promptText.SetActive(false); // Siguradong naka-hide sa start
        }
    }

    private void Update()
    {
        // Kung walang player o prompt text, huwag ituloy para iwas error
        if (playerTransform == null || promptText == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= interactDistance)
        {
            if (!promptText.activeSelf) 
            {
                promptText.SetActive(true);
            }

            if (promptTextMesh != null)
            {
                promptTextMesh.text = "[F] Pick up Access Card";
            }

            if (Camera.main != null && promptRect != null)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 0.5f);
                promptRect.position = screenPos;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                PickupCard();
            }
        }
        else
        {
            if (promptText.activeSelf)
            {
                promptText.SetActive(false);
            }
        }
    }

    private void PickupCard()
    {
        if (promptText != null) promptText.SetActive(false);

        GameObject door = GameObject.Find("doors");
        if (door != null) door.SetActive(false);

        GameObject objTextGo = GameObject.Find("ComputerObjectiveText");
        if (objTextGo != null)
        {
            TMP_Text objText = objTextGo.GetComponent<TMP_Text>();
            if (objText != null) objText.text = "Door Unlocked!";
        }

        Destroy(gameObject);
    }
}