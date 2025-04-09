using UnityEngine;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour
{
    private static GameOptions instance;

    private bool _showDialogueText = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        set => _showDialogueText = value;
    }
}
