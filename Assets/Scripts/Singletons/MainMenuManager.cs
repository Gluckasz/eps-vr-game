using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }

    public static GameObject mainMenuInstance;
    public static GameObject optionsMenuInstance;

    public GameObject mainMenu;
    public GameObject optionsMenu;

    public InputActionProperty openOptionsButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        ShowMainMenu();
    }

    private void Update()
    {
        if (
            openOptionsButton.action.WasPressedThisFrame()
            && SceneManager.GetActiveScene().buildIndex > 0
        )
        {
            ToggleOptionsMenu();
        }
    }

    private void OnEnable()
    {
        if (openOptionsButton.action != null)
        {
            openOptionsButton.action.Enable();
        }
        else
        {
            Debug.LogError(
                "MainMenuManager: 'openOptionsButton' action is not assigned or is null.",
                this
            );
        }
    }

    private void OnDisable()
    {
        if (openOptionsButton.action != null)
        {
            openOptionsButton.action.Disable();
        }
    }

    private void ToggleLocomotion()
    {
        Debug.Log("Toggling locomotion");
        LocomotionDisabler.Instance.enableMovement = !LocomotionDisabler.Instance.enableMovement;
        LocomotionDisabler.Instance.enableTurning = !LocomotionDisabler.Instance.enableTurning;
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
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Debug.Log("Showing Main Menu");
            ToggleLocomotion();
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
    }

    public void ToggleOptionsMenu()
    {
        Debug.Log("Showing Options Menu");
        ToggleLocomotion();
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
            optionsMenuInstance.SetActive(!optionsMenuInstance.activeSelf);
        }

        if (mainMenuInstance != null)
        {
            mainMenuInstance.SetActive(false);
        }
    }
}
