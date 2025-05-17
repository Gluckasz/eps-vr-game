using System.Collections.Generic;
using UnityEngine;

public class ChoiceDialogue : DialogueCollection, Dialogue
{
    public ChoiceDialogue(List<DialogueNode> dialogueNodes)
        : base(dialogueNodes) { }

    public DialogueIterator CreateDialogueIterator()
    {
        return new ChoiceDialogueIterator(this);
    }
}
