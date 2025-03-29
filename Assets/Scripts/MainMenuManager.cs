using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject xROrigin;
    void Start()
    {
        LocomotionDisabler locomotionDisablerScript = xROrigin.GetComponent<LocomotionDisabler>();

        locomotionDisablerScript.enableMovement = false;
        locomotionDisablerScript.enableTurning = false;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
