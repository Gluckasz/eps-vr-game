using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BasicDialogue : DialogueCollection, Dialogue
{
    public BasicDialogue(List<DialogueNode> dialogueNodes)
        : base(dialogueNodes) { }

    public DialogueIterator CreateDialogueIterator()
    {
        return new BasicDialogueIterator(this);
    }
}
