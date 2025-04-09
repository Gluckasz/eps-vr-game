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
    public float spawnDistance = 1.0f; // How far in front of the player to spawn items

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
        if (spawnButton != null)
        {
            spawnButton.onClick.RemoveListener(RetrieveItem);
        }
    }

    void Update()
    {
        bool isButtonPressed = toggleInventoryAction.action.ReadValue<float>() > 0.5f;

        if (isButtonPressed && !wasButtonPressed)
        {
            ToggleInventory();
        }
        wasButtonPressed = isButtonPressed;

        if (isInventoryVisible)
        {
            UpdateInventoryPosition();
        }
    }

    void UpdateInventoryPosition()
    {
        Vector3 headPosition = player.position;
        Vector3 forward = player.forward;

        
        //Vector3 targetPosition = headPosition + forward * 0.6f - new Vector3(0, 0.2f, 0);

        //inventoryUI.transform.position = Vector3.Lerp(inventoryUI.transform.position, targetPosition, Time.deltaTime * 10f);
        //inventoryUI.transform.rotation = Quaternion.Slerp(inventoryUI.transform.rotation, Quaternion.LookRotation(forward), Time.deltaTime * 10f);
    }

    void ToggleInventory()
    {
        isInventoryVisible = !isInventoryVisible;
        inventoryUI.SetActive(isInventoryVisible);

        if (isInventoryVisible)
        {
            UpdateInventoryPosition(); // Set position immediately
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
        if (itemDropdown.options.Count == 0)
            return;

        string selectedItem = itemDropdown.options[itemDropdown.value].text;
        if (inventoryItems.ContainsKey(selectedItem))
        {
            GameObject item = inventoryItems[selectedItem];
            item.SetActive(true);

            // Calculate spawn position a bit further away
            Vector3 spawnPosition = player.position + player.forward * spawnDistance;

            // Set position and add slight random rotation
            item.transform.position = spawnPosition;
            item.transform.rotation = Quaternion.Euler(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));

            // Remove the item from inventory
            inventoryItems.Remove(selectedItem);
            itemDropdown.options.RemoveAt(itemDropdown.value);

            // Update dropdown
            if (itemDropdown.options.Count > 0)
            {
                int newIndex = Mathf.Clamp(itemDropdown.value, 0, itemDropdown.options.Count - 1);
                itemDropdown.value = newIndex;
            }

            itemDropdown.RefreshShownValue();
        }
    }
}
