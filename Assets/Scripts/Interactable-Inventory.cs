using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InventorySystem : MonoBehaviour
{
    public List<GameObject> inventory = new List<GameObject>();

    public void AddToInventory(GameObject item)
    {
        if (!inventory.Contains(item))
        {
            inventory.Add(item);
            item.SetActive(false); // Hide item from world
            Debug.Log($"Added {item.name} to inventory");
        }
    }
}

public class InventoryItem : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void Update()
    {
        if (isHeld && Input.GetKeyDown(KeyCode.I)) // Change to proper XR input later
        {
            FindObjectOfType<InventorySystem>().AddToInventory(gameObject);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isHeld = false;
    }
}
