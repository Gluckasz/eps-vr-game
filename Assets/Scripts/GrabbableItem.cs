using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbableInspectable : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    private void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab)
            grab.selectEntered.AddListener(OnGrabbed);
    }

    private void OnDestroy()
    {
        if (grab)
            grab.selectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        // Start inspecting
        ItemInspectionManager.instance.StartInspection(gameObject);

        // Optionally, disable grabbing after discovery
        grab.enabled = false;

    }
}
