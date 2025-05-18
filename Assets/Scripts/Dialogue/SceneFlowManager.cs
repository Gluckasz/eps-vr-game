using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class SceneFlowManager : MonoBehaviour
{
    private AudioSource audioSource;
    private DialogueReader choiceDialogueReader;
    private DialogueIterator sceneDialogueIterator;
    public static SceneFlowManager Instance { get; private set; }
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

    public void ChoiceDialogueNextNode(
        DialogueDisplay dialogueDisplay,
        string nextId,
        Vector3 dialoguePosition
    )
    {
        if (sceneDialogueIterator.HasMore(nextId))
        {
            dialogueDisplay.ToggleNextButton(false);

            sceneDialogueIterator.SetId(nextId);
            DialogueNode nextNode = sceneDialogueIterator.GetNode();
            dialogueDisplay.DisplayData(nextNode, dialoguePosition);
            dialogueDisplay.ToggleDisplay(true);
        }
        else
        {
            dialogueDisplay.ToggleDisplay(false);
        }
    }

    public void ShowSceneDialogue(Vector3 dialoguePosition)
    {
        DialogueData sceneScript = choiceDialogueReader.ReadJsonDialogueData(sceneScriptFileName);
        Dialogue sceneDialogue = new ChoiceDialogue(sceneScript.dialogue);
        DialogueDisplay dialogueDisplay = choiceDialogueReader.CreateDialogueDisplay(
            sceneScript.dialogue[0].character
        );
        sceneDialogueIterator = sceneDialogue.CreateDialogueIterator();

        ChoiceDialogueNextNode(dialogueDisplay, entryId, dialoguePosition);
    }
}
