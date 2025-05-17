using UnityEngine;

public interface DialogueDisplay
{
    public void HideDisplay();
    public void HideNextButton();
    public void DisplayData(DialogueNode dialogueNode, Vector3 position);
}
