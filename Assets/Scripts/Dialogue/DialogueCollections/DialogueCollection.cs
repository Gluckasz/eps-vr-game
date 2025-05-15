using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public abstract class DialogueCollection
{
    public readonly Dictionary<string, DialogueNode> dialogueNodes = new();

    public DialogueCollection(List<DialogueNode> dialogueNodes)
    {
        foreach (var dialogueNode in dialogueNodes)
        {
            this.dialogueNodes[dialogueNode.id] = dialogueNode;
        }
    }
}
