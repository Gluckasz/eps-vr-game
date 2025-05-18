using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BasicDialogueDisplay : MonoBehaviour, DialogueDisplay
{
    private DialogueNode dialogueNode_;
    private DialogueIterator dialogueIterator_;

    public TMP_Text dialogueText;
    public Button nextButton;
    public GameObject textDisplay;

    private string ConstructDialogueText(DialogueNode node)
    {
        return node.character + ": " + node.text;
    }

    public void ToggleDisplay(bool active)
    {
        textDisplay.gameObject.SetActive(active);
    }

    public void ToggleNextButton(bool active)
    {
        nextButton.gameObject.SetActive(active);
    }

    public void DisplayData(DialogueNode dialogueNode, Vector3 position)
    {
        if (dialogueNode.choices.Count > 4)
        {
            Debug.LogError("Viewing above 4 choices is not supported.");
        }
        dialogueNode_ = dialogueNode;
        dialogueText.text = ConstructDialogueText(dialogueNode);
        gameObject.transform.position = position;
    }

    public void OnNextButtonPressed()
    {
        // Can be later changed from transform.position to characters position
        // (if characters will be moving in the dialogue)
        SceneFlowManager.Instance.BasicDialogueNextNode(this, transform.position);
    }

    public void SetDialogueIterator(DialogueIterator dialogueIterator)
    {
        if (dialogueIterator_ == null)
        {
            dialogueIterator_ = dialogueIterator;
        }
        else
        {
            Debug.LogError("Dialogue iterator already set.");
        }
    }

    public DialogueIterator GetDialogueIterator()
    {
        return dialogueIterator_;
    }
}
