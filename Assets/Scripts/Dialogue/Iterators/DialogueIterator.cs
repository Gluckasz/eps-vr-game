using UnityEngine;

public interface DialogueIterator
{
    public DialogueNode GetNode();
    public bool HasMore(string id);
    public void SetId(string newId);
}
