using System.IO;
using UnityEngine;

public abstract class DialogueReader
{
    private DialogueData dialogueData_;
    private const string textsFolder = "Scene1Texts";

    public abstract DialogueDisplay CreateDialogueDisplay();

    public void ReadJsonDialogueData(string fileName)
    {
        string filePath = Path.Combine(Application.dataPath, textsFolder, fileName);

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            dialogueData_ = JsonUtility.FromJson<DialogueData>(jsonContent);

            Debug.Log("Dialogue from: " + filePath + " loaded successfully!");
        }
        else
        {
            Debug.LogError("Could not find JSON file at path: " + filePath);
        }
    }
}
