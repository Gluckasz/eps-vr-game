using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButtonDisplay : MonoBehaviour
{
    private readonly ChoiceDialogueDisplay choiceDialogueDisplay_;
    private DialogueChoiceNode dialogueChoiceNode_;

    public TMP_Text choiceText;

    public ChoiceButtonDisplay(
        ChoiceDialogueDisplay choiceDialogueDisplay,
        DialogueChoiceNode dialogueChoiceNode
    )
    {
        choiceDialogueDisplay_ = choiceDialogueDisplay;
        dialogueChoiceNode_ = dialogueChoiceNode;

        choiceText.text = dialogueChoiceNode_.shortText;
    }

    public void SetDialogueChoice(DialogueChoiceNode dialogueChoiceNode)
    {
        dialogueChoiceNode_ = dialogueChoiceNode;

        choiceText.text = dialogueChoiceNode_.shortText;
    }

    public void OnChoiceButtonPressed()
    {
        throw new NotImplementedException();
    }
}
