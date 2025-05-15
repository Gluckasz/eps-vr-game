using UnityEngine;

public interface DialogueIterator
{
    public DialogueNode GetNode(string nodeId);
    public bool HasMore(string nodeId);
}
