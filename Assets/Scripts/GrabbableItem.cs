using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabbableItem : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectExited.AddListener(OnSelectExited);
        }
        else
        {
            Debug.LogError("No XRGrabInteractable component found on " + gameObject.name);
        }
    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (ItemInspectionManager.Instance != null)
        {
            ItemInspectionManager.Instance.StartInspection(gameObject);
        }
        else
        {
            Debug.LogWarning("No ItemInspectionManager found in the scene");
        }
    }
}
