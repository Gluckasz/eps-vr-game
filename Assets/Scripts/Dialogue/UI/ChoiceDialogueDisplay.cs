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
    private string nextId;
    private List<ChoiceButtonDisplay> choiceButtons_ = new();
    private DialogueNode dialogueNode_;
    private DialogueIterator dialogueIterator_;

    private const string playerName = "You";
    private const string choiceButtonName = "ChoiceButton";
    private const string audioDir = "AIVoiceAudio";

    private Dictionary<string, Vector3> characterOffsetMap = new()
    {
        { "Father", new(-0.5f, 1.4f, -0.2f) },
        { "Mother", new(0.5f, 1.3f, -0.2f) },
        { "Sibling", new(0.3f, 1.3f, 0.6f) },
        { "Narrator", new(-3.94f, 1.2f, -1f) },
        { "Credits", new(-3.94f, 1.2f, -1f) },
        { "Feedback", new(0, 0, 0.3f) },
    };
    private readonly List<string> characterTags = new()
    {
        "Player",
        "Sibling",
        "Mother",
        "Father",
        "Feedback",
        "Narrator",
    };

    public float choiceXOffset = 0.5f;
    public float choiceYOffset = -0.1f;
    public float choiceYMargin = 2f;
    public float choicezOffset = -0.1f;
    public TMP_Text dialogueText;
    public Button nextButton;
    public GameObject choiceButtonGameObject;
    public GameObject textDisplay;

    private string ConstructDialogueText(DialogueNode node)
    {
        return node.character + ": " + node.text;
    }

    private string ConstructDialogueText(DialogueChoiceNode node)
    {
        return playerName + ": " + node.text;
    }

    private void UpdateChoicesButtons()
    {
        int choiceButtonsCount = choiceButtons_.Count;
        for (int i = 0; i < dialogueNode_.choices.Count - choiceButtonsCount; i++)
        {
            choiceButtons_.Add(Instantiate(Resources.Load<ChoiceButtonDisplay>(choiceButtonName)));
        }

        for (int i = 0; i < choiceButtons_.Count && i < dialogueNode_.choices.Count; i++)
        {
            if (i >= dialogueNode_.choices.Count)
            {
                choiceButtons_[i].gameObject.SetActive(false);
                continue;
            }
            choiceButtons_[i].SetDialogueChoice(dialogueNode_.choices[i], this);
            choiceButtons_[i].gameObject.SetActive(true);
        }
    }

    private void UpdateChoicesButtonsTransforms(int choicesToUpdate)
    {
        for (int i = 1; i <= choicesToUpdate; i++)
        {
            GameObject father = GameObject.FindGameObjectWithTag("Father");
            Vector3 fatherOffset = characterOffsetMap["Father"];

            float newXPosition;
            if (i % 2 == 1)
            {
                newXPosition = father.transform.position.x + (-1) * choiceXOffset + fatherOffset.x;
            }
            else
            {
                newXPosition = father.transform.position.x + choiceXOffset + fatherOffset.x;
            }

            float newYPosition =
                father.transform.position.y
                + (i + 1) / 2 * choiceYOffset
                + choiceYMargin
                + fatherOffset.y;

            Vector3 newPosition = new(
                newXPosition,
                newYPosition,
                father.transform.position.z + choicezOffset + fatherOffset.z
            );
            choiceButtons_[i - 1].gameObject.transform.position = newPosition;
        }
    }

    private void HideChoicesButtons()
    {
        foreach (var choiceButton in choiceButtons_)
        {
            choiceButton.gameObject.SetActive(false);
        }
    }

    private void StopAllCharacterAudio()
    {
        foreach (string tag in characterTags)
        {
            if (GameObject.FindGameObjectWithTag(tag) is GameObject character)
            {
                if (character.TryGetComponent<AudioSource>(out AudioSource audioSource))
                {
                    audioSource.Stop();
                }
            }
        }
    }

    private void PlayAudio(DialogueNode dialogueNode)
    {
        StopAllCharacterAudio();

        GameObject character = GameObject.FindGameObjectWithTag(dialogueNode.character);
        AudioSource characterAudioSource = character.GetComponent<AudioSource>();

        if (dialogueNode.audio != null)
        {
            string audioPath = Path.Combine(audioDir, dialogueNode.character, dialogueNode.audio);

            AudioClip clip = Resources.Load<AudioClip>(audioPath);
            if (clip != null)
            {
                characterAudioSource.clip = clip;
                characterAudioSource.Play();
            }
            else
            {
                Debug.LogWarning($"AudioClip not found at path: {audioPath}");
            }
        }
        else
        {
            Debug.LogWarning($"Audio path not found in node with id: {dialogueNode.id}");
        }
    }

    private void PlayAudio(DialogueChoiceNode dialogueChoiceNode)
    {
        StopAllCharacterAudio();

        GameObject character = GameObject.FindGameObjectWithTag("Player");
        AudioSource characterAudioSource = character.GetComponent<AudioSource>();

        if (dialogueChoiceNode.audio != null)
        {
            string audioPath = Path.Combine(audioDir, "Player", dialogueChoiceNode.audio);

            AudioClip clip = Resources.Load<AudioClip>(audioPath);
            if (clip != null)
            {
                characterAudioSource.clip = clip;
                characterAudioSource.Play();
            }
            else
            {
                Debug.LogWarning($"AudioClip not found at path: {audioPath}");
            }
        }
        else
        {
            Debug.LogWarning(
                $"Audio path not found in choice with shortText: {dialogueChoiceNode.shortText}"
            );
        }
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
        if (dialogueNode.choices.Count > 0)
        {
            ToggleNextButton(false);
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

        UpdateChoicesButtons();

        UpdateChoicesButtonsTransforms(choiceButtons_.Count);

        PlayAudio(dialogueNode_);
        SceneFlowManager.Instance.PlayTalkAnimation(dialogueNode_.character);
        Debug.Log($"Played talk animation for: {dialogueNode_.character}");
    }

    public void ChoiceSelected(DialogueChoiceNode selectedChoice)
    {
        HideChoicesButtons();
        dialogueText.text = ConstructDialogueText(selectedChoice);
        nextId = selectedChoice.nextId;
        ToggleNextButton(true);
        PlayAudio(selectedChoice);
        SceneFlowManager.Instance.PlayIdleAnimation(dialogueNode_.character);
    }

    public void OnNextButtonPressed()
    {
        if (dialogueNode_.nextId != null)
        {
            nextId = dialogueNode_.nextId;
        }
        if (nextId == "exit")
        {
            Debug.Log("Quitting the app");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        Vector3 offset = characterOffsetMap[dialogueNode_.character];

        StartCoroutine(SceneFlowManager.Instance.ChoiceDialogueNextNode(this, nextId));
    }

    public void SetDialogueIterator(DialogueIterator dialogueIterator)
    {
        dialogueIterator_ = dialogueIterator;
    }

    public DialogueIterator GetDialogueIterator()
    {
        return dialogueIterator_;
    }
}
