using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class SceneFlowManager : MonoBehaviour
{
    private AudioSource audioSource;
    private DialogueReader choiceDialogueReader;
    private static SceneFlowManager Instance;
    private const string voiceActingDirectory = "AIVoiceAudio";
    private const string sceneScriptFileName = "Scene1Dialogue.json";
    private const string entryId = "entry";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        choiceDialogueReader = ChoiceDialogueReader.Instance;
    }

    private void Start()
    {
        Vector3 pos = new(-3, 1.3f, -3);
        ShowSceneDialogue(pos);
    }

    private void DialogueSetup(
        Dialogue dialogue,
        DialogueReader dialogueReader,
        DialogueDisplay dialogueDisplay,
        Vector3 dialoguePosition
    )
    {
        DialogueIterator sceneDialogueIterator = dialogue.CreateDialogueIterator();

        sceneDialogueIterator.SetId(entryId);

        DialogueNode startNode = sceneDialogueIterator.GetNode();

        dialogueDisplay.DisplayData(startNode, dialoguePosition);
    }

    public void ShowSceneDialogue(Vector3 dialoguePosition)
    {
        DialogueData sceneScript = choiceDialogueReader.ReadJsonDialogueData(sceneScriptFileName);
        Dialogue sceneDialogue = new ChoiceDialogue(sceneScript.dialogue);
        DialogueDisplay dialogueDisplay = choiceDialogueReader.CreateDialogueDisplay(
            sceneScript.dialogue[0].character
        );

        DialogueSetup(sceneDialogue, choiceDialogueReader, dialogueDisplay, dialoguePosition);
    }
}
