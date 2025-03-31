using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public Slider slider;
    public TMP_Text valueText;

    private void Start()
    {
        UpdateText(slider.value);

        slider.onValueChanged.AddListener(UpdateText);
    }

    private void UpdateText(float value)
    {
        valueText.text = Mathf.RoundToInt(value).ToString();
    }
}
