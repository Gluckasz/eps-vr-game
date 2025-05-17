using System;
using UnityEngine;

public class BasicDialogueIterator : DialogueIterator
{
    private readonly DialogueCollection dialogue_;
    private string currentId_;

    public BasicDialogueIterator(DialogueCollection dialogue)
    {
        dialogue_ = dialogue;
    }

    public DialogueNode GetNode()
    {
        if (HasMore(currentId_))
        {
            DialogueNode result = dialogue_.dialogueNodes[currentId_];
            currentId_ = result.nextId;
            return result;
        }
        else
        {
            return null;
        }
    }

    public bool HasMore(string id)
    {
        return dialogue_.dialogueNodes.ContainsKey(id);
    }

    public void SetId(string newId)
    {
        currentId_ = newId;
    }
}
