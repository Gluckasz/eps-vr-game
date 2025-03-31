using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ManageVolumeSlider : MonoBehaviour
{
    public Slider slider;

    public AudioMixer audioMixer;
    public string volumeParameter = "MasterVolume";

    private float multiplier = 20f; // For dB conversion

    private void Start()
    {
        // Initialize slider position based on current mixer value
        float mixerValue;
        if (audioMixer != null && audioMixer.GetFloat(volumeParameter, out mixerValue))
        {
            // Convert from dB to normalized slider value
            slider.value = Mathf.Pow(10, mixerValue / multiplier) * 100;
        }
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    private void HandleSliderValueChanged(float value)
    {
        float normalizedValue = value / 100f;

        // Convert slider value (0-100) to logarithmic dB scale
        float dbValue = Mathf.Log10(Mathf.Max(normalizedValue, 0.0001f)) * multiplier;
        audioMixer.SetFloat(volumeParameter, dbValue);
    }
}
