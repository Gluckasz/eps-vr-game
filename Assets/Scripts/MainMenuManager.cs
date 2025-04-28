using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class MainMenuManager : MonoBehaviour
{
    public static GameObject mainMenuInstance;
    public static GameObject optionsMenuInstance;

    public GameObject xROrigin;

    public GameObject mainMenu;
    public GameObject optionsMenu;

    void Start()
    {
        LocomotionDisabler locomotionDisablerScript = xROrigin.GetComponent<LocomotionDisabler>();
        locomotionDisablerScript.enableMovement = false;
        locomotionDisablerScript.enableTurning = false;

        ShowMainMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame()
    {
        if (SaveManager.Instance.HasSaveData)
        {
            SaveManager.Instance.LoadGame();
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ShowMainMenu()
    {
        if (mainMenuInstance == null)
        {
            mainMenuInstance = Instantiate(mainMenu);
            if (mainMenuInstance != null)
            {
                mainMenuInstance.SetActive(true);
            }
        }
        else
        {
            mainMenuInstance.SetActive(true);
        }

        if (optionsMenuInstance != null)
        {
            optionsMenuInstance.SetActive(false);
        }
    }

    public void ShowOptionsMenu()
    {
        if (optionsMenuInstance == null)
        {
            optionsMenuInstance = Instantiate(optionsMenu);
            if (optionsMenuInstance != null)
            {
                optionsMenuInstance.SetActive(true);
            }
        }
        else
        {
            optionsMenuInstance.SetActive(true);
        }

        if (mainMenuInstance != null)
        {
            mainMenuInstance.SetActive(false);
        }
    }
}