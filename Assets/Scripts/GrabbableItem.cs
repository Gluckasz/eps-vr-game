using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabbableItem : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private InventoryManager inventory;

    private void Awake()
    {
        // Find the inventory manager once at startup for better performance
        inventory = FindFirstObjectByType<InventoryManager>();

        // Get the XRGrabInteractable component
        grabInteractable = GetComponent<XRGrabInteractable>();

        // Subscribe to the select exited event
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
        // Always unsubscribe when the object is destroyed
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (inventory != null)
        {
            inventory.AddItemToInventory(gameObject);
        }
        else
        {
            Debug.LogWarning("No InventoryManager found in the scene");
        }
    }
}