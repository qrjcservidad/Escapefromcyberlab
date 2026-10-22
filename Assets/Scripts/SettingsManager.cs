// 1. Dito DAPAT nakalagay LAHAT ng using directives sa PINAKATAAS:
using UnityEngine;
using UnityEngine.UI;

// 2. Pagkatapos ng using statements, saka pa lang ang class:
public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider;
    public Slider brightnessSlider;
    public Image brightnessOverlay;

    private void Awake()
    {
        // I-apply agad ang saved settings pagkabukas na pagkabukas ng GameObject
        ApplySavedSettings();
    }

    private void OnEnable()
    {
        // Panatilihing updated ang Sliders at Effects tuwing nag-a-activate ang Panel
        ApplySavedSettings();
    }

    void Start()
    {
        // I-link ang mga event listeners sa sliders
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.RemoveAllListeners();
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }
    }

    public void ApplySavedSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 1f);

        // I-update ang UI sliders nang hindi nagko-cause ng redundant calls
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }

        if (brightnessSlider != null)
        {
            brightnessSlider.value = savedBrightness;
        }

        // I-apply sa system
        SetVolume(savedVolume);
        SetBrightness(savedBrightness);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save(); // Siguraduhing nai-save sa storage
    }

    public void SetBrightness(float value)
    {
        if (brightnessOverlay != null)
        {
            Color alphaColor = brightnessOverlay.color;
            // Value 0 = madilim (alpha 0.8), Value 1 = maliwanag (alpha 0)
            alphaColor.a = (1f - value) * 0.8f; 
            brightnessOverlay.color = alphaColor;
        }
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save(); // Siguraduhing nai-save sa storage
    }
}