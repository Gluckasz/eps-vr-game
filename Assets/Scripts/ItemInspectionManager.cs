using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class ItemInspectionManager : MonoBehaviour
{
    public static ItemInspectionManager instance;

    public GameObject inspectionCanvas;
    public Transform itemHolder;
    public TextMeshProUGUI descriptionText;
    public Button continueButton;
    public Image itemImageUI;
    public TextMeshProUGUI itemName;

    private GameObject currentItem;

    private Camera cam;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        cam = Camera.main;
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

        // Set UI info
        descriptionText.text = inspectable.description;
        itemName.text = inspectable.itemName;

        if (inspectable.itemImage != null)
        {
            itemImageUI.sprite = inspectable.itemImage;
            itemImageUI.enabled = true;
        }
        else
        {
            itemImageUI.enabled = false;
        }

        // Set canvas position above player's head
        Vector3 headPos = cam.transform.position + Vector3.up * 0.2f;
        Vector3 forward = cam.transform.forward;
        Vector3 canvasPos = headPos + forward * 1.2f;

        // Raycast to avoid spawning into walls
        if (Physics.Raycast(headPos, forward, out RaycastHit hit, 1.2f))
        {
            canvasPos = hit.point - forward * 0.1f;
        }

        inspectionCanvas.transform.position = canvasPos;

        // Rotate canvas to face the player without tilting (yaw only)
        Vector3 directionToPlayer = inspectionCanvas.transform.position - cam.transform.position;
        directionToPlayer.y = 0; // remove tilt
        if (directionToPlayer.sqrMagnitude > 0.01f)
        {
            inspectionCanvas.transform.rotation = Quaternion.LookRotation(directionToPlayer.normalized, Vector3.up);
        }


        // Disable LazyFollow rotation
        LazyFollow lazy = inspectionCanvas.GetComponent<LazyFollow>();
        if (lazy != null)
        {
            lazy.rotationFollowMode = LazyFollow.RotationFollowMode.None;
        }

        inspectionCanvas.SetActive(true);

        // Spawn the item inside the canvas holder
        currentItem = Instantiate(item, itemHolder.position, itemHolder.rotation, itemHolder);
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        // Discover item
        ItemDiscoveryManager.instance.DiscoverItem(inspectable.itemName);
    }

    public void CloseInspection()
    {
        if (currentItem != null)
            Destroy(currentItem);

        inspectionCanvas.SetActive(false);
    }
}
