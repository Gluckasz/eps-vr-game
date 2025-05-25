using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Makes an object trigger the intro dialogue when hovered and clicked in VR.
/// </summary>
public class DialogueTriggerInteractable : XRBaseInteractable
{
    private new bool isHovered = false;
    private Color originalColor;

    public Renderer objectRenderer;

    protected override void Awake()
    {
        base.Awake();
        if (objectRenderer != null)
            originalColor = objectRenderer.material.color;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        hoverEntered.AddListener(OnHoverEntered);
        hoverExited.AddListener(OnHoverExited);
        selectEntered.RemoveListener(OnSelectEntered);
    }

    protected override void OnDisable()
    {
        hoverEntered.RemoveListener(OnHoverEntered);
        hoverExited.RemoveListener(OnHoverExited);
        activated.RemoveListener(OnActivated);
        base.OnDisable();
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        isHovered = true;
        Debug.Log($"Hover entered on {gameObject.name}");

        if (objectRenderer != null)
            objectRenderer.material.color = Color.yellow;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        isHovered = false;
        Debug.Log($"Hover exited on {gameObject.name}");

        if (objectRenderer != null)
            objectRenderer.material.color = originalColor;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log($"Selected (grabbed) {gameObject.name}");
        TriggerDialogue();
    }

    private void TriggerDialogue()
    {
        if (isHovered && tag == "Mother")
        {
            SceneFlowManager.Instance.ShowMotherIntroDialogue();
        }
    }
}
