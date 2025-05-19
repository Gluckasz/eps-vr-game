using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class SceneFlowManager : MonoBehaviour
{
    private AudioSource audioSource;

    private DialogueReader choiceDialogueReader;
    private DialogueReader basicDialogueReader;

    private const string voiceActingDirectory = "AIVoiceAudio";
    private const string sceneScriptFileName = "Scene1Dialogue.json";
    private const string introScriptFileName = "Scene1Intro.json";
    private const string entryId = "entry";

    public static SceneFlowManager Instance { get; private set; }

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
        basicDialogueReader = BasicDialogueReader.Instance;
    }

    private void Start()
    {
        Vector3 pos = new(-3, 1.3f, -3);
        ShowSceneDialogue(pos);
        pos = new(-1, 1, 0);
        ShowIntroDialogue(pos);
    }

    public void ChoiceDialogueNextNode(
        DialogueDisplay dialogueDisplay,
        string nextId,
        Vector3 dialoguePosition
    )
    {
        DialogueIterator dialogueIterator = dialogueDisplay.GetDialogueIterator();
        if (dialogueIterator.HasMore(nextId))
        {
            dialogueIterator.SetId(nextId);
            DialogueNode nextNode = dialogueIterator.GetNode();
            dialogueDisplay.DisplayData(nextNode, dialoguePosition);
            dialogueDisplay.ToggleDisplay(true);
            if (nextNode.nextId == null)
            {
                dialogueDisplay.ToggleNextButton(false);
            }
            else
            {
                dialogueDisplay.ToggleNextButton(true);
            }
        }
        else
        {
            dialogueDisplay.ToggleDisplay(false);
        }
    }

    public void BasicDialogueNextNode(DialogueDisplay dialogueDisplay, Vector3 dialoguePosition)
    {
        DialogueIterator dialogueIterator = dialogueDisplay.GetDialogueIterator();
        DialogueNode nextNode = dialogueIterator.GetNode();
        if (nextNode != null)
        {
            dialogueDisplay.ToggleNextButton(true);

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

        DialogueIterator sceneDialogueIterator = sceneDialogue.CreateDialogueIterator();
        dialogueDisplay.SetDialogueIterator(sceneDialogueIterator);

        ChoiceDialogueNextNode(dialogueDisplay, entryId, dialoguePosition);
    }

    public void ShowIntroDialogue(Vector3 dialoguePosition)
    {
        DialogueData introScript = basicDialogueReader.ReadJsonDialogueData(introScriptFileName);
        Dialogue introDialogue = new BasicDialogue(introScript.dialogue);
        DialogueDisplay dialogueDisplay = basicDialogueReader.CreateDialogueDisplay(
            introScript.dialogue[0].character
        );

        DialogueIterator introDialogueIterator = introDialogue.CreateDialogueIterator();
        introDialogueIterator.SetId(entryId);
        dialogueDisplay.SetDialogueIterator(introDialogueIterator);

        BasicDialogueNextNode(dialogueDisplay, dialoguePosition);
    }
}
