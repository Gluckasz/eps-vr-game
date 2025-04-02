using System.IO;
using UnityEngine;

public class SceneFlowManager : MonoBehaviour
{
    public DialogueData dialogueData;
    public string scriptPath = "scene_1_script.json";

    void Start()
    {
        LoadDialogueData();
    }

    void LoadDialogueData()
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
}
