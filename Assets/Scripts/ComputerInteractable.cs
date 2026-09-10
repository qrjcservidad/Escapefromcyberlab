using UnityEngine;

public class ComputerInteractable : MonoBehaviour
{
    public GameObject computerSystemPanel;

    private void Update()
    {
        // Kapag pinindot ang E, lalabas agad ang UI Panel
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenComputerUI();
        }
    }

    public void OpenComputerUI()
    {
        if (computerSystemPanel != null)
        {
            computerSystemPanel.SetActive(true);
        }
    }
}