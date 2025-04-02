using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryUI; // The inventory UI panel
    public Transform player; // Reference to the player
    public TMP_Dropdown itemDropdown; // The dropdown UI for items
    public Transform itemSpawnPoint; // Where retrieved items appear

    private Dictionary<string, GameObject> inventoryItems = new Dictionary<string, GameObject>();
    private bool isInventoryVisible = false;

    void Start()
    {
        inventoryUI.SetActive(false); // Hide inventory at start
    }

    void Update()
    {
        // Toggle inventory with button (adjust the input based on your system)
        //if (XRInputManager.Instance.IsButtonPressed(XRInputManager.Buttons.SecondaryButton))
        //{
        //    ToggleInventory();
        //}

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
