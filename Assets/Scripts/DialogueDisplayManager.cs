using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class DialogueDisplayManager : MonoBehaviour
{
    private List<DialogueChoice> choiceData_ = new();
    private DialogueData dialogueData_;
    private Dictionary<string, DialogueNode> dialogueNodes = new Dictionary<string, DialogueNode>();
    private string playerName = "You";
    private string nextId_;
    private GameOptions gameOptionsScript;
    private AudioSource audioSource_;
    private bool isIntroPlaying = false;
    private bool isFeedbackDisplayed_ = false;
    private Dictionary<string, string> endFeedbackMap = new Dictionary<string, string>
    {
        { "end1", "feedback1.txt" },
        { "end2", "feedback2.txt" },
        { "end3", "feedback3.txt" },
        { "end4", "feedback4.txt" },
    };

    public TMP_Dropdown choicesDropdown;
    public TMP_Text dialogueText;
    public Button nextButton;
    public GameObject gameOptionsManager;
    public GameObject textDisplay;
    public string feedbackDirName = "Scene1Feedback";


    private void Start()
    {
        gameOptionsScript = gameOptionsManager.GetComponent<GameOptions>();

        textDisplay.SetActive(gameOptionsScript.ShowDialogueText);
    }

    private string ConstructDialogueText(DialogueNode node)
    {
        return node.character + ": " + node.text;
    }

    private string ConstructDialogueText(DialogueChoice choice)
    {
        return playerName + ": " + choice.text;
    }

    private void DisplayPlayerText(DialogueChoice dialogueChoice)
    {
        UpdateText(ConstructDialogueText(dialogueChoice));
    }

    private IEnumerator WaitForAudioToFinish(float delay)
    {
        yield return new WaitForSeconds(delay + 0.5f);
        if (isIntroPlaying)
        {
            isIntroPlaying = false;
        }
    }

    private void DisplayFeedback()
    {
        textDisplay.SetActive(true);

        string filePath = Path.Combine(Application.dataPath, feedbackDirName, endFeedbackMap[nextId_]);
        if (File.Exists(filePath))
        {
            string feedback = File.ReadAllText(filePath);
            UpdateText(feedback);
        }
        else
        {
            Debug.LogError("Could not find TXT file at path: " + filePath);
        }
    }

    public void StartSceneDialogue()
    {
        foreach (var node in dialogueData_.scene)
        {
            dialogueNodes[node.id] = node;
        }

        Debug.Log($"Loaded {dialogueNodes.Count} dialogue nodes successfully");

        nextButton.gameObject.SetActive(false);
        choicesDropdown.gameObject.SetActive(true);

        UpdateText(ConstructDialogueText(dialogueData_.scene[0]));

        AddOptionsFromChoices(dialogueData_.scene[0].choices);
    }

    public void UpdateText(string text)
    {
        dialogueText.text = text;
    }

    // Max 50 characters to dropdown option
    public void AddOptionToDropdown(DialogueChoice choice)
    {
        TMP_Dropdown.OptionData newOption = new();
        newOption.text = choice.shortText;

        choicesDropdown.options.Add(newOption);

        choiceData_.Add(choice);

        choicesDropdown.RefreshShownValue();
    }

    public void AddOptionsFromChoices(List<DialogueChoice> choices)
    {
        ClearDropdownOptions();

        TMP_Dropdown.OptionData newOption = new();
        newOption.text = "Choose an answer";

        choicesDropdown.options.Add(newOption);
        foreach (var choice in choices)
        {
            AddOptionToDropdown(choice);
        }
    }

    public DialogueChoice GetSelectedChoice()
    {
        int selectedIndex = choicesDropdown.value;
        if (selectedIndex >= 1 && selectedIndex < choiceData_.Count + 1)
        {
            return choiceData_[selectedIndex - 1];
        }
        return null;
    }

    public void ClearDropdownOptions()
    {
        choicesDropdown.ClearOptions();
        choiceData_.Clear();
    }

    public void OnDropdownValueChanged()
    {
        DialogueChoice selectedChoice = GetSelectedChoice();
        if (selectedChoice != null)
        {
            Debug.Log($"Selected: {selectedChoice.text}, Next ID: {selectedChoice.nextId}");
            nextId_ = selectedChoice.nextId;

            choicesDropdown.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);

            if (endFeedbackMap.ContainsKey(nextId_))
            {
                isFeedbackDisplayed_ = true;
                DisplayFeedback();
                return;
            }


            DisplayPlayerText(selectedChoice);
        }
    }

    public void OnNextButtonPressed()
    {
        nextButton.gameObject.SetActive(false);
        if (isFeedbackDisplayed_)
        {
            // TODO: Go to next level
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        if (isIntroPlaying)
        {
            audioSource_.Stop();
            isIntroPlaying = false;
            // TODO: Start scene dialogue after putting out the dishes
            StartSceneDialogue();
            return;
        }
        choicesDropdown.gameObject.SetActive(true);

        UpdateText(ConstructDialogueText(dialogueNodes[nextId_]));

        AddOptionsFromChoices(dialogueNodes[nextId_].choices);
    }

    public void StartScene(DialogueData dialogueData, AudioSource audioSource)
    {
        dialogueData_ = dialogueData;
        audioSource_ = audioSource;

        string audioPath = dialogueData.intro[0].audio;

        string resourcePath = audioPath.Replace("Assets/", "").Replace(".wav", "");

        AudioClip introClip = Resources.Load<AudioClip>(resourcePath);

        if (introClip != null)
        {
            choicesDropdown.gameObject.SetActive(false);
            UpdateText(ConstructDialogueText(dialogueData.intro[0]));

            audioSource_.clip = introClip;
            audioSource_.Play();
            isIntroPlaying = true;
            nextButton.gameObject.SetActive(true);

            StartCoroutine(WaitForAudioToFinish(introClip.length));
        }
        else
        {
            Debug.LogError("Could not load audio clip from: " + resourcePath);
        }
    }
}
