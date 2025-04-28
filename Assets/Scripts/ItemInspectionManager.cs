using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInspectionManager : MonoBehaviour
{
    public static ItemInspectionManager instance;

    public GameObject inspectionCanvas;
    public Transform playerHead;
    public Transform itemHolder; // Empty GameObject in canvas where item spawns
    public TextMeshProUGUI descriptionText;
    public Button continueButton;

    private GameObject currentItem;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        inspectionCanvas.SetActive(false);

        continueButton.onClick.AddListener(CloseInspection);
    }

    public void StartInspection(GameObject item)
    {
        InspectableItem inspectable = item.GetComponent<InspectableItem>();
        if (inspectable == null)
        {
            Debug.LogError("Item has no InspectableItem script attached!");
            return;
        }

        item.SetActive(false);

        descriptionText.text = inspectable.description;

        inspectionCanvas.SetActive(true);
        UpdateCanvasPosition();

        currentItem = Instantiate(item, itemHolder.position, itemHolder.rotation, itemHolder);
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true; // Disable physics while inspecting

        currentItem.transform.LookAt(playerHead);
        currentItem.transform.Rotate(0f, 180f, 0f);

        ItemDiscoveryManager.instance.DiscoverItem(inspectable.itemName);
    }


    private void Update()
    {
        if (inspectionCanvas.activeSelf)
        {
            UpdateCanvasPosition();
        }
    }

    void UpdateCanvasPosition()
    {
        Vector3 spawnPos = playerHead.position + playerHead.forward * 1.0f; // 1 meter in front
        inspectionCanvas.transform.position = spawnPos;
        inspectionCanvas.transform.rotation = Quaternion.LookRotation(playerHead.forward);
    }

    public void CloseInspection()
    {
        if (currentItem != null)
            Destroy(currentItem);

        inspectionCanvas.SetActive(false);
    }
}
