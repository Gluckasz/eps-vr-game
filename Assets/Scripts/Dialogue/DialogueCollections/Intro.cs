using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Intro : Dialogue
{
    public readonly Dictionary<string, DialogueNode> dialogueNodes_ = new();

    public Intro(List<DialogueNode> dialogueNodes)
    {
        foreach (var dialogueNode in dialogueNodes)
        {
            dialogueNodes_[dialogueNode.id] = dialogueNode;
        }
    }

    public DialogueIterator CreateDialogueIterator()
    {
        return new IntroIterator(this);
    }
}
