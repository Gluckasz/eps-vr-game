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
    public Transform itemSpawnPoint; // Where retrieved items appear
    public Button spawnButton; // Reference to the spawn button

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

        // Keep inventory near the player
        if (isInventoryVisible)
        {
            inventoryUI.transform.position = player.position + player.forward * 0.5f;
            inventoryUI.transform.rotation = Quaternion.LookRotation(player.forward);
        }
    }

    void ToggleInventory()
    {
        isInventoryVisible = !isInventoryVisible;
        inventoryUI.SetActive(isInventoryVisible);
    }

    // Your existing methods...
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
        string selectedItem = itemDropdown.options[itemDropdown.value].text;
        if (inventoryItems.ContainsKey(selectedItem))
        {
            GameObject item = inventoryItems[selectedItem];
            item.SetActive(true);
            item.transform.position = itemSpawnPoint.position;
        }
    }
}