using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplayManager : MonoBehaviour
{
    private List<DialogueChoice> choiceData_ = new();
    private DialogueData dialogueData_;
    private Dictionary<string, DialogueNode> dialogueNodes = new Dictionary<string, DialogueNode>();
    private string playerName = "You";
    private string nextId_;
    private GameOptions gameOptionsScript;

    public TMP_Dropdown choicesDropdown;
    public TMP_Text dialogueText;
    public Button nextButton;
    public GameObject gameOptionsManager;
    public GameObject textDisplay;


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
            nextButton.gameObject.SetActive(true);
            choicesDropdown.gameObject.SetActive(false);
            DisplayPlayerText(selectedChoice);
        }
    }

    public void OnNextButtonPressed()
    {
        nextButton.gameObject.SetActive(false);
        choicesDropdown.gameObject.SetActive(true);

        UpdateText(ConstructDialogueText(dialogueNodes[nextId_]));

        AddOptionsFromChoices(dialogueNodes[nextId_].choices);
    }

    public void StartSceneDialogue(DialogueData dialogueData)
    {
        dialogueData_ = dialogueData;

        foreach (var node in dialogueData_.scene)
        {
            dialogueNodes[node.id] = node;
        }

        Debug.Log($"Loaded {dialogueNodes.Count} dialogue nodes successfully");

        nextButton.gameObject.SetActive(false);

        UpdateText(ConstructDialogueText(dialogueData_.scene[0]));

        AddOptionsFromChoices(dialogueData_.scene[0].choices);
    }
}
