using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class IdentificationQuestion
{
    public string questionText;
    public string correctAnswer;
}

public class IdentificationQuizManager : MonoBehaviour
{
    [Header("UI Panels & Controls")]
    public GameObject computerUI;
    public Button proceedButton;
    public GameObject quizPanel;

    [Header("Quiz UI Elements")]
    public TMP_Text questionText;
    public TMP_InputField answerInputField;
    public Button submitButton;
    public TMP_Text feedbackText;

    [Header("Penalty Settings")]
    public float penaltySeconds = 20f;

    [Header("State")]
    public bool hasAccessCard = false;
    private int currentQuestionIndex = 0;

    [Header("Questions List")]
    public List<IdentificationQuestion> questions = new List<IdentificationQuestion>();

    private void Start()
    {
        if (proceedButton != null)
        {
            proceedButton.interactable = false;
        }

        if (quizPanel != null) quizPanel.SetActive(false);
        if (feedbackText != null) feedbackText.text = "";

        if (questions.Count == 0)
        {
            LoadDefaultQuestions();
        }

        if (submitButton != null)
        {
            submitButton.onClick.RemoveAllListeners();
            submitButton.onClick.AddListener(CheckAnswer);
        }
    }

    public void UnlockProceedButton()
    {
        hasAccessCard = true;
        if (proceedButton != null)
        {
            proceedButton.interactable = true;
        }
    }

    public void OnProceedButtonClicked()
    {
        if (!hasAccessCard) return;

        if (computerUI != null) computerUI.SetActive(false);
        if (quizPanel != null) quizPanel.SetActive(true);

        currentQuestionIndex = 0;
        if (feedbackText != null) feedbackText.text = "";
        DisplayQuestion();
    }

    private void DisplayQuestion()
    {
        // KAPAG NATAPOS AT TAMA LAHAT NG TANONG
        if (currentQuestionIndex >= questions.Count)
        {
            if (feedbackText != null) 
                feedbackText.text = "<color=green>QUIZ COMPLETED! DOORS UNLOCKED!</color>";

            if (quizPanel != null) 
                quizPanel.SetActive(false);

            // TAWAGIN ANG SCENE3QUEST PARA MASIRA O MAWALA ANG PINTO
            Scene3Quest quest = FindFirstObjectByType<Scene3Quest>(FindObjectsInactive.Include);
            if (quest != null)
            {
                quest.OpenDoors();
                Debug.Log("IdentificationQuizManager: Correct all answers! Calling OpenDoors() on Scene3Quest.");
            }
            else
            {
                Debug.LogError("IdentificationQuizManager ERROR: Hindi nahanap ang Scene3Quest sa Scene!");
            }

            return;
        }

        IdentificationQuestion q = questions[currentQuestionIndex];
        if (questionText != null)
        {
            questionText.text = (currentQuestionIndex + 1) + ". " + q.questionText;
        }
        
        if (answerInputField != null)
        {
            answerInputField.text = "";
            answerInputField.Select();
            answerInputField.ActivateInputField();
        }
    }

    public void CheckAnswer()
    {
        if (answerInputField == null) return;

        string playerAnswer = answerInputField.text.Trim();
        IdentificationQuestion q = questions[currentQuestionIndex];

        if (string.Equals(playerAnswer, q.correctAnswer, System.StringComparison.OrdinalIgnoreCase))
        {
            if (feedbackText != null) feedbackText.text = "<color=green>Correct!</color>";
            currentQuestionIndex++;
            DisplayQuestion();
        }
        else
        {
            if (feedbackText != null) feedbackText.text = $"<color=red>Wrong! -{penaltySeconds}s (Reset to Q1)</color>";
            
            // BAWASAN ANG ORAS SA SCENE3QUEST
            Scene3Quest quest = FindFirstObjectByType<Scene3Quest>(FindObjectsInactive.Include);
            if (quest != null)
            {
                quest.DeductTime(penaltySeconds);
            }

            currentQuestionIndex = 0;
            DisplayQuestion();
        }
    }

    private void LoadDefaultQuestions()
    {
        questions.Add(new IdentificationQuestion { questionText = "Physical, tangible components of a computer system.", correctAnswer = "Hardware" });
        questions.Add(new IdentificationQuestion { questionText = "Set of instructions, data, or programs used to operate computers.", correctAnswer = "Software" });
        questions.Add(new IdentificationQuestion { questionText = "Main system software that manages computer hardware and software resources.", correctAnswer = "Operating System" });
        questions.Add(new IdentificationQuestion { questionText = "Software designed to detect, prevent, and remove malware.", correctAnswer = "Antivirus" });
        questions.Add(new IdentificationQuestion { questionText = "An individual who uses computer skills to gain unauthorized access to data.", correctAnswer = "Hacker" });
        questions.Add(new IdentificationQuestion { questionText = "Simulation of human intelligence processes by machines (Abbreviation).", correctAnswer = "AI" });
        questions.Add(new IdentificationQuestion { questionText = "Primary component that executes instructions and processes data (Abbreviation).", correctAnswer = "CPU" });
        questions.Add(new IdentificationQuestion { questionText = "Software program designed to perform specific user-oriented tasks.", correctAnswer = "Application" });
        questions.Add(new IdentificationQuestion { questionText = "Popular desktop Operating System developed by Microsoft.", correctAnswer = "Windows" });
        questions.Add(new IdentificationQuestion { questionText = "Volatile memory used to store working data currently in execution.", correctAnswer = "RAM" });
    }
}