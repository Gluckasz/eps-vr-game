using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    public static OptionsMenuManager Instance { get; private set; }
    public Button goBackButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ToggleBackButton(bool active)
    {
        goBackButton.gameObject.SetActive(active);
    }
}
