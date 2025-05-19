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

    private Dictionary<string, Vector3> characterOffsetMap = new()
    {
        { "Father", new(-0.5f, 1.6f, -0.2f) },
        { "Mother", new(0.5f, 1.6f, -0.2f) },
        { "Sibling", new(0, 1.6f, 0.4f) },
        { "Narrator", new(0, 1.3f, 0) },
    };

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

    public void DisplayData(DialogueNode dialogueNode)
    {
        if (dialogueNode.choices.Count > 4)
        {
            Debug.LogError("Viewing above 4 choices is not supported.");
        }
        dialogueNode_ = dialogueNode;
        dialogueText.text = ConstructDialogueText(dialogueNode);

        GameObject targetGameObject = GameObject.FindGameObjectWithTag(dialogueNode_.character);
        Vector3 offset = characterOffsetMap[dialogueNode_.character];

        Vector3 newPosition = new(
            targetGameObject.transform.position.x + offset.x,
            targetGameObject.transform.position.y + offset.y,
            targetGameObject.transform.position.z + offset.z
        );
        transform.position = newPosition;
    }

    public void OnNextButtonPressed()
    {
        // Can be later changed from transform.position to characters offset
        // (if characters will be moving in the dialogue)
        SceneFlowManager.Instance.BasicDialogueNextNode(this);
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
