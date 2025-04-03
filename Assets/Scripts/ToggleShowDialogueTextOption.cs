using UnityEngine;
using UnityEngine.UI;

public class ToggleShowDialogueTextOption : MonoBehaviour
{
    private static GameOptions gameOptionsScript;

    public GameObject gameOptionsManager;
    public Toggle showDialogueTextOptionToggle;

    private void Start()
    {
        gameOptionsScript = gameOptionsManager.GetComponent<GameOptions>();
        gameOptionsScript.ShowDialogueText = showDialogueTextOptionToggle.isOn;
    }
    public void Toggle()
    {
        gameOptionsScript.ShowDialogueText = !gameOptionsScript.ShowDialogueText;

        Debug.Log("Changed show dialogue text variable to: " + gameOptionsScript.ShowDialogueText);
    }
}
