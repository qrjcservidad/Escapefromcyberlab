using UnityEngine;

public class ClueManager : MonoBehaviour
{
    // Bubuksan o isasara nito ang nakatagong ClueUI parent o papatayin ang active clues
    public void InspectAllClues()
    {
        // Kung nakatago ang ClueUI parent, ilalabas nito
        gameObject.SetActive(!gameObject.activeSelf);
    }

    // Pwede ring tawagin ito para isara lahat ng child clues
    public void CloseAllClues()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}