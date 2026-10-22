using UnityEngine;
using TMPro; // Kailangan para sa TextMeshPro UI

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;

    public static int keysCollected = 0;
    [SerializeField] private int totalKeysNeeded = 3;
    [SerializeField] private TextMeshProUGUI keyCounterText; // UI Text Display

    private void Awake()
    {
        // Singleton pattern para madaling ma-access
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        UpdateKeyUI();
    }

    public void AddKey()
    {
        keysCollected++;
        UpdateKeyUI();
        Debug.Log("Keys Collected: " + keysCollected + "/" + totalKeysNeeded);
    }

    public void UpdateKeyUI()
    {
        if (keyCounterText != null)
        {
            keyCounterText.text = "Keys Picked Up: " + keysCollected + "/" + totalKeysNeeded;
        }
    }
}