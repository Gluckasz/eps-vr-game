using UnityEngine;
using UnityEngine.UI;

public class ToggleShowDialogueTextOption : MonoBehaviour
{
    private bool isInitializing = false;

    public GameObject gameOptionsManager;
    public Toggle showDialogueTextOptionToggle;

    private void Awake()
    {
        isInitializing = true;
        showDialogueTextOptionToggle.isOn = GameOptions.Instance.ShowDialogueText;
        isInitializing = false;
    }

    public void Toggle()
    {
        if (!isInitializing)
        {
            GameOptions.Instance.ShowDialogueText = !GameOptions.Instance.ShowDialogueText;

            Debug.Log(
                "Changed show dialogue text variable to: " + GameOptions.Instance.ShowDialogueText
            );
        }
    }
}
