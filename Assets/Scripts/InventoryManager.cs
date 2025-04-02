using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryUI; // The inventory UI panel
    public Transform player; // Reference to the player
    public TMP_Dropdown itemDropdown; // The dropdown UI for items
    public Button spawnButton; // Reference to the spawn button
    public float spawnDistance = 0.5f; // How far in front of the player to spawn items

    public InputActionProperty toggleInventoryAction;

    private Dictionary<string, GameObject> inventoryItems = new Dictionary<string, GameObject>();
    private bool isInventoryVisible = false;
    private bool wasButtonPressed = false;

    void Start()
    {
        inventoryUI.SetActive(false); // Hide inventory at start

        // Enable the action
        toggleInventoryAction.action.Enable();

        // Connect the spawn button to the RetrieveItem method
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(RetrieveItem);
        }
        else
        {
            Debug.LogWarning("Spawn button reference is missing!");
        }
    }

    void OnDestroy()
    {
        // Clean up the listener when the object is destroyed
        if (spawnButton != null)
        {
            spawnButton.onClick.RemoveListener(RetrieveItem);
        }
    }

    void Update()
    {
        // Check for button press (with state change detection to avoid toggling every frame)
        bool isButtonPressed = toggleInventoryAction.action.ReadValue<float>() > 0.5f;

        if (isButtonPressed && !wasButtonPressed)
        {
            ToggleInventory();
        }
        wasButtonPressed = isButtonPressed;

        // Update inventory position only when it's visible
        if (isInventoryVisible)
        {
            // Calculate position in front of player's view
            Vector3 headPosition = player.position;
            Vector3 forward = player.forward;

            // Ensure the UI is positioned at a fixed distance in front of the player
            Vector3 targetPosition = headPosition + forward * 0.5f;

            // Use smoothing to prevent jittering
            inventoryUI.transform.position = Vector3.Lerp(inventoryUI.transform.position, targetPosition, Time.deltaTime * 10f);

            // Calculate rotation to face the player directly
            Quaternion targetRotation = Quaternion.LookRotation(forward);
            inventoryUI.transform.rotation = Quaternion.Slerp(inventoryUI.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void ToggleInventory()
    {
        isInventoryVisible = !isInventoryVisible;
        inventoryUI.SetActive(isInventoryVisible);

        // If we're showing the inventory, immediately position it correctly
        if (isInventoryVisible)
        {
            // Set initial position and rotation
            Vector3 headPosition = player.position;
            Vector3 forward = player.forward;

            inventoryUI.transform.position = headPosition + forward * 0.5f;
            inventoryUI.transform.rotation = Quaternion.LookRotation(forward);
        }
    }

    public void AddItemToInventory(GameObject item)
    {
        string itemName = item.name;
        if (!inventoryItems.ContainsKey(itemName))
        {
            inventoryItems[itemName] = item;
            itemDropdown.options.Add(new TMP_Dropdown.OptionData(itemName));
            itemDropdown.RefreshShownValue();
        }
        item.SetActive(false); // Hide the item from the world
    }

    public void RetrieveItem()
    {
        // Check if there are any items in the dropdown
        if (itemDropdown.options.Count == 0)
            return;

        string selectedItem = itemDropdown.options[itemDropdown.value].text;
        if (inventoryItems.ContainsKey(selectedItem))
        {
            GameObject item = inventoryItems[selectedItem];
            item.SetActive(true);

            // Calculate spawn position in front of the player
            Vector3 spawnPosition = player.position + player.forward * spawnDistance;

            // Set the item's position
            item.transform.position = spawnPosition;

            // Set the item's rotation to match the player's view
            item.transform.rotation = player.rotation;

            // Remove the item from the inventory dictionary
            inventoryItems.Remove(selectedItem);

            // Remove the item from the dropdown
            int currentIndex = itemDropdown.value;
            itemDropdown.options.RemoveAt(currentIndex);

            // Update the dropdown selection
            if (itemDropdown.options.Count > 0)
            {
                // Select the next item, or the last item if we were at the end
                int newIndex = currentIndex < itemDropdown.options.Count ? currentIndex : itemDropdown.options.Count - 1;
                itemDropdown.value = newIndex;
            }

            // Refresh the dropdown display
            itemDropdown.RefreshShownValue();
        }
    }
}