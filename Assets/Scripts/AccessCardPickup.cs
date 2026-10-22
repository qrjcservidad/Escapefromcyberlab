using UnityEngine;
using TMPro;

public class AccessCardPickup : MonoBehaviour
{
    [Header("Drag Objects Here")]
    public GameObject promptText;
    public Transform playerTransform;

    [Header("Settings")]
    public float interactDistance = 4f;

    private RectTransform promptRect;
    private TMP_Text promptTextMesh;

    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) 
            {
                playerTransform = playerObj.transform;
            }
        }

        if (promptText != null)
        {
            promptRect = promptText.GetComponent<RectTransform>();
            promptTextMesh = promptText.GetComponent<TMP_Text>();
            promptText.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= interactDistance)
        {
            if (promptText != null)
            {
                if (!promptText.activeSelf) promptText.SetActive(true);

                if (promptTextMesh != null)
                {
                    promptTextMesh.text = "[F] Pick up Access Card";
                }

                if (Camera.main != null && promptRect != null)
                {
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 0.5f);
                    promptRect.position = screenPos;
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                PickupCard();
            }
        }
        else
        {
            if (promptText != null && promptText.activeSelf)
            {
                promptText.SetActive(false);
            }
        }
    }

    public void OnPickupButtonPressed()
    {
        if (playerTransform == null)
        {
            PickupCard();
            return;
        }

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= interactDistance)
        {
            PickupCard();
        }
    }

    public void PickupCard()
    {
        if (promptText != null) promptText.SetActive(false);

        // TAWAIN ANG IDENTIFICATION QUIZ MANAGER LANG
        IdentificationQuizManager identQuiz = FindFirstObjectByType<IdentificationQuizManager>(FindObjectsInactive.Include);
        if (identQuiz != null)
        {
            identQuiz.UnlockProceedButton();
            Debug.Log("Access Card Picked Up: Proceed Button Unlocked sa IdentificationQuizManager!");
        }

        TutorialManager tutorial = FindFirstObjectByType<TutorialManager>(FindObjectsInactive.Include);
        if (tutorial != null)
        {
            tutorial.CompletePickupCardTask();
        }

        Destroy(gameObject);
    }
}