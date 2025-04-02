using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplayManager : MonoBehaviour
{
    private List<DialogueChoice> choiceData = new();

    public TMP_Dropdown choicesDropdown;
    public TMP_Text dialogueText;

    public void UpdateText(string text)
    {
        dialogueText.text = text;
    }

    public void AddOptionToDropdown(DialogueChoice choice)
    {
        TMP_Dropdown.OptionData newOption = new();
        newOption.text = choice.shortText;

        choicesDropdown.options.Add(newOption);

        choiceData.Add(choice);

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
        if (selectedIndex >= 1 && selectedIndex < choiceData.Count + 1)
        {
            return choiceData[selectedIndex - 1];
        }
        return null;
    }

    public void ClearDropdownOptions()
    {
        choicesDropdown.ClearOptions();
        choiceData.Clear();
    }

    public void OnDropdownValueChanged()
    {
        DialogueChoice selectedChoice = GetSelectedChoice();
        if (selectedChoice != null)
        {
            Debug.Log($"Selected: {selectedChoice.text}, Next ID: {selectedChoice.nextId}");
        }
    }
}
