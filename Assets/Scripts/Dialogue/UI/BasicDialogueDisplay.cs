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
    private const string audioDir = "AIVoiceAudio";

    private Dictionary<string, Vector3> characterOffsetMap = new()
    {
        { "Father", new(0.2f, 1.4f, -0.2f) },
        { "Mother", new(0.5f, 1.2f, -0.2f) },
        { "Sibling", new(0, 1.3f, 0.4f) },
        { "Narrator", new(0, 1.3f, 0) },
        { "Player", new(0, 1f, 1.5f) },
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

    public TMP_Text dialogueText;
    public Button nextButton;
    public GameObject textDisplay;

    private string ConstructDialogueText(DialogueNode node)
    {
        return node.character + ": " + node.text;
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

        transform.rotation = targetGameObject.transform.rotation;
        transform.position = targetGameObject.transform.position;

        transform.Translate(offset);

        SceneFlowManager.Instance.PlayTalkAnimation(dialogueNode_.character);
        PlayAudio(dialogueNode_);
    }

    public void OnNextButtonPressed()
    {
        SceneFlowManager.Instance.PlayIdleAnimation(dialogueNode_.character);
        SceneFlowManager.Instance.BasicDialogueNextNode(this);
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
