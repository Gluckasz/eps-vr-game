using System.IO;
using UnityEngine;

public class SceneFlowManager : MonoBehaviour
{
    private static DialogueDisplayManager dialogueDisplayManagerScript;

    public DialogueData dialogueData;
    public string scriptPath = "scene_1_script.json";
    public GameObject dialogueDisplayer;

    public static GameObject dialogueDisplayerInstance;


    private void Start()
    {
        LoadDialogueData();

        dialogueDisplayerInstance = Instantiate(dialogueDisplayer);
        dialogueDisplayerInstance.SetActive(false);

        dialogueDisplayManagerScript = dialogueDisplayerInstance.GetComponent<DialogueDisplayManager>();

        StartScene();
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
                Debug.Log("First node text: " + dialogueData.intro[0].text);
            }
        }
        else
        {
            Debug.LogError("Could not find JSON file at path: " + filePath);
        }
    }

    public void StartScene()
    {
        dialogueDisplayManagerScript.UpdateText(dialogueData.scene[0].text);

        dialogueDisplayManagerScript.AddOptionsFromChoices(dialogueData.scene[0].choices);

        dialogueDisplayerInstance.SetActive(true);
    }
}
