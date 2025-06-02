using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Makes an object trigger the intro dialogue when hovered and clicked in VR.
/// </summary>
public class DialogueTriggerInteractable : XRBaseInteractable
{
    private new bool isHovered = false;
    private const string motherIntroScriptFileName = "MotherIntro.json";
    private const string fatherIntroScriptFileName = "FatherIntro.json";
    private const string siblingIntroScriptFileName = "SiblingIntro.json";

    public Renderer objectRenderer;

    protected override void Awake()
    {
        base.Awake();
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
        if (!SceneFlowManager.Instance.SceneDialougePlaying)
        {
            isHovered = true;
            Debug.Log($"Hover entered on {gameObject.name}");
        }
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        if (!SceneFlowManager.Instance.SceneDialougePlaying)
        {
            isHovered = false;
            Debug.Log($"Hover exited on {gameObject.name}");
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!SceneFlowManager.Instance.SceneDialougePlaying)
        {
            Debug.Log($"Selected {gameObject.name}");
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
        if (isHovered && !SceneFlowManager.Instance.SceneDialougePlaying)
        {
            if (tag == "Mother")
            {
                SceneFlowManager.Instance.ShowCharacterIntroDialogue(motherIntroScriptFileName);
            }
            else if (tag == "Father")
            {
                SceneFlowManager.Instance.ShowCharacterIntroDialogue(fatherIntroScriptFileName);
            }
            else if (tag == "Sibling")
            {
                SceneFlowManager.Instance.ShowCharacterIntroDialogue(siblingIntroScriptFileName);
            }
        }
    }
}
