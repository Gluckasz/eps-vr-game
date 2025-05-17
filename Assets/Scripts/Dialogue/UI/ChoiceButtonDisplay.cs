using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButtonDisplay : MonoBehaviour
{
    private ChoiceDialogueDisplay choiceDialogueDisplay_;
    private DialogueChoiceNode dialogueChoiceNode_;

    public TMP_Text choiceText;

    private void Start()
    {
        choiceText.text = dialogueChoiceNode_.shortText;
    }

    public void SetDialogueChoice(
        DialogueChoiceNode dialogueChoiceNode,
        ChoiceDialogueDisplay choiceDialogueDisplay
    )
    {
        choiceDialogueDisplay_ = choiceDialogueDisplay;

        dialogueChoiceNode_ = dialogueChoiceNode;

        choiceText.text = dialogueChoiceNode_.shortText;
    }

    public void OnChoiceButtonPressed()
    {
        choiceDialogueDisplay_.ChoiceSelected(dialogueChoiceNode_);
    }
}
