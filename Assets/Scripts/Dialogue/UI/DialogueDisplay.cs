using UnityEngine;

public interface DialogueDisplay
{
    public void HideDisplay();
    public void ShowDisplay();
    public void HideNextButton();
    public void ShowNextButton();
    public void DisplayData(DialogueNode dialogueNode, Vector3 position);
}
