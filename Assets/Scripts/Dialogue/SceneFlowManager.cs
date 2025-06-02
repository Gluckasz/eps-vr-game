using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class SceneFlowManager : MonoBehaviour
{
    private AudioSource audioSource;

    private DialogueReader choiceDialogueReader;
    private DialogueReader basicDialogueReader;

    private const string sceneScriptFileName = "Scene1Dialogue.json";
    private const string introScriptFileName = "Scene1Intro.json";
    private const string introReminderScriptFileName = "NarratorIntroReminder.json";
    private const string entryId = "entry";

    public bool SceneDialougePlaying { get; private set; } = false;
    public int introReminderWaitTime = 90;

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
        StartCoroutine(ShowIntroReminder());
    }

    private IEnumerator ShowIntroReminder()
    {
        yield return new WaitForSecondsRealtime(introReminderWaitTime);
        if (!SceneDialougePlaying)
        {
            DialogueData script = basicDialogueReader.ReadJsonDialogueData(
                introReminderScriptFileName
            );
            Dialogue dialogue = new BasicDialogue(script.dialogue);
            DialogueDisplay dialogueDisplay = basicDialogueReader.CreateDialogueDisplay(
                script.dialogue[0].character
            );

            DialogueIterator dialogueIterator = dialogue.CreateDialogueIterator();
            dialogueIterator.SetId(entryId);
            dialogueDisplay.SetDialogueIterator(dialogueIterator);

            BasicDialogueNextNode(dialogueDisplay);
            StartCoroutine(ShowIntroReminder());
        }
    }

    public void PlayTalkAnimation(string characterTag)
    {
        GameObject character = GameObject.FindGameObjectWithTag(characterTag);
        Animator animator = character.GetComponent<Animator>();
        if (SceneDialougePlaying)
        {
            switch (characterTag)
            {
                case "Father":
                    animator.Play("FatherDialogueTalking");
                    break;
                case "Mother":
                    animator.Play("MotherDialogueTalking");
                    break;
                case "Sibling":
                    animator.Play("SiblingDialogueTalking");
                    break;
            }
        }
        else
        {
            switch (characterTag)
            {
                case "Father":
                    animator.Play("FatherIntroTalking");
                    break;
                case "Mother":
                    animator.Play("MotherIntroTalking");
                    break;
                case "Sibling":
                    animator.Play("SiblingIntroTalking");
                    break;
            }
        }
    }

    public void PlayIdleAnimation(string characterTag)
    {
        GameObject character = GameObject.FindGameObjectWithTag(characterTag);
        if (character != null)
        {
            Animator animator = character.GetComponent<Animator>();

            if (SceneDialougePlaying)
            {
                switch (characterTag)
                {
                    case "Father":
                        animator.Play("FatherDialogueIdle");
                        break;
                    case "Mother":
                        animator.Play("MotherDialogueIdle");
                        break;
                    case "Sibling":
                        animator.Play("SiblingDialogueIdle");
                        break;
                }
            }
            else
            {
                switch (characterTag)
                {
                    case "Father":
                        animator.Play("FatherIntro");
                        break;
                    case "Mother":
                        animator.Play("MotherIntro");
                        break;
                    case "Sibling":
                        animator.Play("SiblingIntro");
                        break;
                }
            }
        }
    }

    public IEnumerator ChoiceDialogueNextNode(DialogueDisplay dialogueDisplay, string nextId)
    {
        PlayIdleAnimation("Father");
        PlayIdleAnimation("Mother");
        PlayIdleAnimation("Sibling");
        yield return new WaitForEndOfFrame();
        Debug.Log("Setall character animations to idle.");
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

    public IEnumerator ShowSceneDialogue()
    {
        SceneDialougePlaying = true;
        PlayIdleAnimation("Father");
        PlayIdleAnimation("Mother");
        PlayIdleAnimation("Sibling");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        LocomotionDisabler locomotionDisablerScript = player.GetComponent<LocomotionDisabler>();
        locomotionDisablerScript.enableMovement = false;
        player.transform.position = new Vector3(-3.5f, 0.1f, -4.3f);
        player.transform.rotation = Quaternion.Euler(0, 0, 0);

        yield return new WaitForEndOfFrame();
        BackgroundMusic.Instance.PlayDialogueMusic();

        DialogueData sceneScript = choiceDialogueReader.ReadJsonDialogueData(sceneScriptFileName);
        Dialogue sceneDialogue = new ChoiceDialogue(sceneScript.dialogue);
        DialogueDisplay dialogueDisplay = choiceDialogueReader.CreateDialogueDisplay(
            sceneScript.dialogue[0].character
        );

        DialogueIterator sceneDialogueIterator = sceneDialogue.CreateDialogueIterator();
        dialogueDisplay.SetDialogueIterator(sceneDialogueIterator);

        StartCoroutine(ChoiceDialogueNextNode(dialogueDisplay, entryId));
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
