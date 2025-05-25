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
    private const string motherTag = "Mother";

    public bool SceneDialougePlaying { get; private set; } = false;

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
        ShowIntroDialogue();
    }

    public void ChoiceDialogueNextNode(DialogueDisplay dialogueDisplay, string nextId)
    {
        DialogueIterator dialogueIterator = dialogueDisplay.GetDialogueIterator();
        if (dialogueIterator.HasMore(nextId))
        {
            dialogueIterator.SetId(nextId);
            DialogueNode nextNode = dialogueIterator.GetNode();
            dialogueDisplay.DisplayData(nextNode);
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

    public void BasicDialogueNextNode(DialogueDisplay dialogueDisplay)
    {
        DialogueIterator dialogueIterator = dialogueDisplay.GetDialogueIterator();
        DialogueNode nextNode = dialogueIterator.GetNode();
        if (nextNode != null)
        {
            dialogueDisplay.ToggleNextButton(true);

            dialogueDisplay.DisplayData(nextNode);
            dialogueDisplay.ToggleDisplay(true);
        }
        else
        {
            dialogueDisplay.ToggleDisplay(false);
        }
    }

    public void ShowSceneDialogue()
    {
        SceneDialougePlaying = true;
        GameObject mother = GameObject.FindGameObjectWithTag(motherTag);
        Animator motherAnimator = mother.GetComponent<Animator>();
        motherAnimator.Play("MotherSitting");

        DialogueData sceneScript = choiceDialogueReader.ReadJsonDialogueData(sceneScriptFileName);
        Dialogue sceneDialogue = new ChoiceDialogue(sceneScript.dialogue);
        DialogueDisplay dialogueDisplay = choiceDialogueReader.CreateDialogueDisplay(
            sceneScript.dialogue[0].character
        );

        DialogueIterator sceneDialogueIterator = sceneDialogue.CreateDialogueIterator();
        dialogueDisplay.SetDialogueIterator(sceneDialogueIterator);

        ChoiceDialogueNextNode(dialogueDisplay, entryId);
    }

    public void ShowIntroDialogue()
    {
        DialogueData introScript = basicDialogueReader.ReadJsonDialogueData(introScriptFileName);
        Dialogue introDialogue = new BasicDialogue(introScript.dialogue);
        DialogueDisplay dialogueDisplay = basicDialogueReader.CreateDialogueDisplay(
            introScript.dialogue[0].character
        );

        DialogueIterator introDialogueIterator = introDialogue.CreateDialogueIterator();
        introDialogueIterator.SetId(entryId);
        dialogueDisplay.SetDialogueIterator(introDialogueIterator);

        BasicDialogueNextNode(dialogueDisplay);
    }

    public void ShowCharacterIntroDialogue(string scriptFileName)
    {
        DialogueData script = basicDialogueReader.ReadJsonDialogueData(scriptFileName);
        Dialogue dialogue = new BasicDialogue(script.dialogue);
        DialogueDisplay dialogueDisplay = basicDialogueReader.CreateDialogueDisplay(
            script.dialogue[0].character
        );

        DialogueIterator dialogueIterator = dialogue.CreateDialogueIterator();
        dialogueIterator.SetId(entryId);
        dialogueDisplay.SetDialogueIterator(dialogueIterator);

        BasicDialogueNextNode(dialogueDisplay);
    }
}
