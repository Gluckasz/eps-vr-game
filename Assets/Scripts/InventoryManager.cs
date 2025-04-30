//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using UnityEngine.XR.Interaction.Toolkit;
//using UnityEngine.InputSystem;

//public class InventoryManager : MonoBehaviour
//{
//    public static InventoryManager Instance;

//    public GameObject inventoryUI;
//    public Transform player;
//    public TMP_Dropdown itemDropdown;
//    public Button spawnButton;
//    public float spawnDistance = 1.0f;
//    public InputActionProperty toggleInventoryAction;

//    private Dictionary<string, GameObject> inventoryItems = new Dictionary<string, GameObject>();
//    private bool isInventoryVisible = false;
//    private bool wasButtonPressed = false;
//    private GameObject itemToAddLater;

//    private void Awake()
//    {
//        Instance = this;
//    }

//    private void Start()
//    {
//        inventoryUI.SetActive(false);
//        toggleInventoryAction.action.Enable();

//        if (spawnButton != null)
//        {
//            spawnButton.onClick.AddListener(RetrieveItem);
//        }
//    }

//    private void Update()
//    {
//        bool isButtonPressed = toggleInventoryAction.action.ReadValue<float>() > 0.5f;

//        if (isButtonPressed && !wasButtonPressed)
//        {
//            ToggleInventory();
//        }
//        wasButtonPressed = isButtonPressed;
//    }

//    public void AddItemToInventory(GameObject item)
//    {
//        itemToAddLater = item;
//        ItemInspectionManager.Instance.StartInspection(item);
//    }

//    public void FinishAddingItem()
//    {
//        if (itemToAddLater == null)
//            return;

//        string itemName = itemToAddLater.name;
//        if (!inventoryItems.ContainsKey(itemName))
//        {
//            inventoryItems[itemName] = itemToAddLater;
//            itemDropdown.options.Add(new TMP_Dropdown.OptionData(itemName));
//            itemDropdown.RefreshShownValue();
//        }

//        itemToAddLater.SetActive(false);
//        itemToAddLater = null;
//    }

//    private void ToggleInventory()
//    {
//        isInventoryVisible = !isInventoryVisible;
//        inventoryUI.SetActive(isInventoryVisible);
//    }

//    public void RetrieveItem()
//    {
//        if (itemDropdown.options.Count == 0)
//            return;

//        string selectedItem = itemDropdown.options[itemDropdown.value].text;
//        if (inventoryItems.ContainsKey(selectedItem))
//        {
//            GameObject item = inventoryItems[selectedItem];
//            item.SetActive(true);

//            Vector3 spawnPosition = player.position + player.forward * spawnDistance;
//            item.transform.position = spawnPosition;
//            item.transform.rotation = Quaternion.Euler(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));

//            inventoryItems.Remove(selectedItem);
//            itemDropdown.options.RemoveAt(itemDropdown.value);
//            itemDropdown.RefreshShownValue();
//        }
//    }
//}
