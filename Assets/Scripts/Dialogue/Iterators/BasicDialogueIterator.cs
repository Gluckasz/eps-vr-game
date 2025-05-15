using System;
using UnityEngine;

public class BasicDialogueIterator : DialogueIterator
{
    private readonly DialogueCollection intro_;
    private string currentId_;

    public BasicDialogueIterator(DialogueCollection intro)
    {
        intro_ = intro;
    }

    public DialogueNode GetNode()
    {
        if (HasMore(currentId_))
        {
            DialogueNode result = intro_.dialogueNodes[currentId_];
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
        return intro_.dialogueNodes.ContainsKey(id);
    }

    public void SetId(string newId)
    {
        currentId_ = newId;
    }
}
