using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class QuestionData
{
    [TextArea(2, 4)]
    public string questionText;
    public bool correctAnswer;
}

public class QuizManager : MonoBehaviour
{
    [Header("UI Text Components")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Passcode System UI")]
    [SerializeField] private GameObject quizSectionPanel;
    [SerializeField] private GameObject passcodeSectionPanel;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private TMP_InputField passcodeInputField;
    [SerializeField] private string correctPasscode = "1234"; 
    [TextArea(2, 3)]
    [SerializeField] private string passcodeHint = "HINT: Look for the sticky note near the whiteboard!";

    [Header("Door Objective Reference")]
    [SerializeField] private GameObject doorToUnlock;
    
    [Header("Scene Transition Reference")]
    [SerializeField] private GameObject sceneTransitionZone; // New: Slot para sa SceneTransitionZone

    [Header("Quiz Questions (5 Questions)")]
    [SerializeField] private List<QuestionData> questionList = new List<QuestionData>();

    private int currentQuestionIndex = 0;
    private bool isQuizPassed = false;

    private void OnEnable()
    {
        if (isQuizPassed)
        {
            ShowPasscodeSection();
        }
        else
        {
            ResetQuiz();
        }
    }

    public void ResetQuiz()
    {
        currentQuestionIndex = 0;
        
        if (quizSectionPanel != null) quizSectionPanel.SetActive(true);
        if (passcodeSectionPanel != null) passcodeSectionPanel.SetActive(false);
        if (feedbackText != null) feedbackText.text = "";

        DisplayCurrentQuestion();
    }

    private void DisplayCurrentQuestion()
    {
        if (currentQuestionIndex < questionList.Count)
        {
            if (questionText != null) 
                questionText.text = questionList[currentQuestionIndex].questionText;
                
            if (scoreText != null) 
                scoreText.text = $"Question {currentQuestionIndex + 1} / {questionList.Count}";
        }
    }

    public void SelectTrue() => CheckAnswer(true);
    public void SelectFalse() => CheckAnswer(false);

    private void CheckAnswer(bool userSelection)
    {
        if (currentQuestionIndex >= questionList.Count) return;

        bool correct = questionList[currentQuestionIndex].correctAnswer;

        if (userSelection == correct)
        {
            currentQuestionIndex++;

            if (currentQuestionIndex < questionList.Count)
            {
                if (feedbackText != null)
                {
                    feedbackText.color = Color.green;
                    feedbackText.text = "CORRECT!";
                }
                Invoke(nameof(DisplayCurrentQuestion), 0.5f);
            }
            else
            {
                isQuizPassed = true;
                ShowPasscodeSection();
            }
        }
        else
        {
            if (feedbackText != null)
            {
                feedbackText.color = Color.red;
                feedbackText.text = "INCORRECT! Restarting quiz from Question 1...";
            }
            Invoke(nameof(ResetQuiz), 1.2f);
        }
    }

    private void ShowPasscodeSection()
    {
        if (quizSectionPanel != null) quizSectionPanel.SetActive(false);
        if (passcodeSectionPanel != null) passcodeSectionPanel.SetActive(true);

        if (hintText != null)
        {
            hintText.text = passcodeHint;
        }

        if (feedbackText != null)
        {
            feedbackText.color = Color.green;
            feedbackText.text = "QUIZ COMPLETED! Enter the access code:";
        }
    }

    public void SubmitPasscode()
    {
        if (passcodeInputField != null)
        {
            if (passcodeInputField.text == correctPasscode)
            {
                if (feedbackText != null)
                {
                    feedbackText.color = Color.green;
                    feedbackText.text = "ACCESS GRANTED! DOOR UNLOCKED.";
                }
                UnlockDoor();
            }
            else
            {
                if (feedbackText != null)
                {
                    feedbackText.color = Color.red;
                    feedbackText.text = "WRONG CODE! Try again.";
                }
                passcodeInputField.text = "";
            }
        }
    }

    private void UnlockDoor()
    {
        if (doorToUnlock != null)
        {
            doorToUnlock.SetActive(false); 
            Debug.Log("Door opened successfully!");
        }

        // Auto-activates transition trigger zone
        if (sceneTransitionZone != null)
        {
            sceneTransitionZone.SetActive(true);
            Debug.Log("Scene Transition Zone activated!");
        }
    }
}