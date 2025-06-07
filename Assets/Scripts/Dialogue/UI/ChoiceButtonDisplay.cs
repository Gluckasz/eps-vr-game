using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButtonDisplay : MonoBehaviour
{
    private bool hasItem = false;
    private bool isDiscovered = true;

    private ChoiceDialogueDisplay choiceDialogueDisplay_;
    private DialogueChoiceNode dialogueChoiceNode_;

    public TMP_Text choiceText;
    public GameObject lockIcon;

    public void SetDialogueChoice(
        DialogueChoiceNode dialogueChoiceNode,
        ChoiceDialogueDisplay choiceDialogueDisplay
    )
    {
        choiceDialogueDisplay_ = choiceDialogueDisplay;

        dialogueChoiceNode_ = dialogueChoiceNode;

        choiceText.text = dialogueChoiceNode_.shortText;

        hasItem = dialogueChoiceNode_.item != null;
        isDiscovered =
            !hasItem || ItemDiscoveryManager.instance.HasDiscovered(dialogueChoiceNode_.item);
        Debug.Log(
            $"Button {dialogueChoiceNode_.shortText}: Item='{dialogueChoiceNode_.item}', HasItem={hasItem}, IsDiscovered={isDiscovered}"
        );

        if (hasItem && !isDiscovered)
        {
            lockIcon.SetActive(true);
        }
        else
        {
            lockIcon.SetActive(false);
        }
    }

    public void OnChoiceButtonPressed()
    {
        if (isDiscovered)
        {
            choiceDialogueDisplay_.ChoiceSelected(dialogueChoiceNode_);
        }
    }
}
