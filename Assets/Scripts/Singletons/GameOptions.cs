using UnityEngine;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    public static GameOptions Instance;

    private bool _showDialogueText = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool ShowDialogueText
    {
        get => _showDialogueText;
        set
        {
            _showDialogueText = value;
            //if (
            //    ChoiceDialogueDisplay.Instance != null
            //    && ChoiceDialogueDisplay.Instance.IsDialogueShowed()
            //)
            //{
            //    ChoiceDialogueDisplay.Instance.textDisplay.SetActive(value);
            //}
        }
    }
}
