using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ChoiceDialogueDisplay : MonoBehaviour, DialogueDisplay
{
    private List<ChoiceButtonDisplay> choiceButtons_ = new();
    private DialogueNode dialogueNode_;
    private const string playerName = "You";
    private const string choiceButtonName = "ChoiceButton";

    public float choiceXOffset = 0.5f;
    public float choiceYOffset = -0.2f;
    public float choiceYMargin = 0.15f;
    public float choicezOffset = -0.1f;
    public TMP_Text dialogueText;
    public Button nextButton;
    public GameObject choiceButtonGameObject;
    public GameObject textDisplay;

    private string ConstructDialogueText(DialogueNode node)
    {
        return node.character + ": " + node.text;
    }

    private void InstantiateChoicesButtons()
    {
        foreach (var choice in dialogueNode_.choices)
        {
            choiceButtons_.Add(Instantiate(Resources.Load<ChoiceButtonDisplay>(choiceButtonName)));
        }
        foreach (var choiceButton in choiceButtons_)
        {
            choiceButton.gameObject.SetActive(true);
        }
    }

    private void UpdateChoicesButtons()
    {
        if (dialogueNode_.choices.Count > choiceButtons_.Count)
        {
            for (int i = 0; i < dialogueNode_.choices.Count - choiceButtons_.Count; i++)
            {
                choiceButtons_.Add(
                    Instantiate(Resources.Load<ChoiceButtonDisplay>(choiceButtonName))
                );
                choiceButtons_[i].gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < choiceButtons_.Count; i++)
        {
            if (i >= dialogueNode_.choices.Count)
            {
                choiceButtons_[i].gameObject.SetActive(false);
                continue;
            }
            choiceButtons_[i].SetDialogueChoice(dialogueNode_.choices[i]);
            choiceButtons_[i].gameObject.SetActive(true);
        }
    }

    private int CountActiveChoiceButtons()
    {
        int count = 0;
        foreach (var choiceButton in choiceButtons_)
        {
            if (choiceButton.gameObject.activeSelf)
            {
                count++;
            }
        }

        return count;
    }

    private void UpdateChoicesButtonsTransforms(int choicesToUpdate)
    {
        for (int i = 1; i <= choicesToUpdate; i++)
        {
            float newXPosition;
            if (i % 2 == 1)
            {
                newXPosition = gameObject.transform.position.x + (-1) * choiceXOffset;
            }
            else
            {
                newXPosition = gameObject.transform.position.x + choiceXOffset;
            }

            float newYPosition =
                gameObject.transform.position.y + (i + 1) / 2 * choiceYOffset + choiceYMargin;

            Vector3 newPosition = new(
                newXPosition,
                newYPosition,
                gameObject.transform.position.z + choicezOffset
            );
            choiceButtons_[i - 1].gameObject.transform.position = newPosition;
        }
    }

    public void HideDisplay()
    {
        textDisplay.SetActive(false);
    }

    public void HideNextButton()
    {
        nextButton.gameObject.SetActive(false);
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

        if (choiceButtons_.Count == 0)
        {
            InstantiateChoicesButtons();
        }
        UpdateChoicesButtons();

        int activeChoicesCount = CountActiveChoiceButtons();
        UpdateChoicesButtonsTransforms(activeChoicesCount);
    }

    public void OnNextButtonPressed()
    {
        throw new NotImplementedException();
    }
}
