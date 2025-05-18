using UnityEngine;

public interface DialogueDisplay
{
    public void ToggleDisplay(bool active);
    public void ToggleNextButton(bool active);
    public void DisplayData(DialogueNode dialogueNode, Vector3 position);
}
