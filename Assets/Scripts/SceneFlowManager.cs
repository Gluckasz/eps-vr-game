using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class SceneFlowManager : MonoBehaviour
{
    private AudioSource audioSource;

    private DialogueDisplayManager dialogueDisplayManagerScript;

    public DialogueData dialogueData;
    public string scriptPath = "scene_1_script.json";
    public GameObject dialogueDisplayer;

    public static GameObject dialogueDisplayerInstance;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        dialogueDisplayerInstance = Instantiate(dialogueDisplayer);
        dialogueDisplayerInstance.SetActive(false);

        dialogueDisplayManagerScript =
            dialogueDisplayerInstance.GetComponent<DialogueDisplayManager>();
        LoadDialogueData();
    }

    private void StartScene()
    {
        dialogueDisplayerInstance.SetActive(true);

        dialogueDisplayManagerScript.StartScene(dialogueData, audioSource);
    }

    private void LoadDialogueData()
    {
        string filePath = Path.Combine(Application.dataPath, scriptPath);

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);

            dialogueData = JsonUtility.FromJson<DialogueData>(jsonContent);

            Debug.Log("Dialogue loaded successfully!");
            if (dialogueData.intro.Count > 0)
            {
                Debug.Log("Starting the scene");
                StartScene();
            }
        }
        else
        {
            Debug.LogError("Could not find JSON file at path: " + filePath);
        }
    }
}
