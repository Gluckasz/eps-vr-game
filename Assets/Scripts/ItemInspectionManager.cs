using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ItemInspectionManager : MonoBehaviour
{
    public static ItemInspectionManager instance;

    public GameObject inspectionCanvas;
    public Transform itemHolder;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI FulldescriptionText;

    public GameObject itemImageObject;
    public GameObject shortDescriptionPanel;
    public GameObject fullDescriptionPanel;
    public Button readMoreButton;
    public Button backButton;
    public Button continueButton;

    public Image itemImageUI;
    public TextMeshProUGUI itemName;

    public float canvasOffsetY = 0.3f;

    private GameObject currentItem;
    private Camera cam;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        cam = Camera.main;
        inspectionCanvas.SetActive(false);
        continueButton.onClick.AddListener(CloseInspection);

        readMoreButton.onClick.AddListener(ShowFullDescription);
        backButton.onClick.AddListener(ShowShortDescription);

        // Default UI state
        fullDescriptionPanel.SetActive(false);
        backButton.gameObject.SetActive(false);
        readMoreButton.gameObject.SetActive(true);
        shortDescriptionPanel.SetActive(true);
    }

    public void StartInspection(GameObject item)
    {
        InspectableItem inspectable = item.GetComponent<InspectableItem>();
        if (inspectable == null)
        {
            Debug.LogError("Item has no InspectableItem script attached!");
            return;
        }

        if (inspectable.itemPosition == null)
        {
            Debug.LogError("No 'itemPosition' set on InspectableItem: " + item.name);
            return;
        }

        // Hide original item for now
        item.SetActive(false);

        // Set UI data
        descriptionText.text = inspectable.description;
        itemName.text = inspectable.itemName;
        FulldescriptionText.text = inspectable.fullDescription;

        if (inspectable.itemImage != null)
        {
            itemImageUI.sprite = inspectable.itemImage;
            itemImageUI.enabled = true;
        }
        else
        {
            itemImageUI.enabled = false;
        }

        // 📌 Position canvas at the custom item position
        Vector3 spawnPos = inspectable.itemPosition.position + Vector3.up * canvasOffsetY;
        inspectionCanvas.transform.position = spawnPos;

        // 👀 Face canvas toward camera
        Vector3 toCam = cam.transform.position - spawnPos;
        toCam.y = 0;
        if (toCam != Vector3.zero)
            inspectionCanvas.transform.rotation = Quaternion.LookRotation(toCam);

        // 🧼 Turn off LazyFollow if present
        LazyFollow lazy = inspectionCanvas.GetComponent<LazyFollow>();
        if (lazy != null)
        {
            lazy.rotationFollowMode = LazyFollow.RotationFollowMode.None;
        }

        // Reset UI state
        ShowShortDescription();
        inspectionCanvas.SetActive(true);

        // Clone the item into the holder
        currentItem = Instantiate(item, itemHolder.position, itemHolder.rotation, itemHolder);
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        XRGrabInteractable grab = currentItem.GetComponent<XRGrabInteractable>();
        if (grab)
        {
            grab.enabled = false;
        }

        foreach (var col in currentItem.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        // Log discovery
        ItemDiscoveryManager.instance.DiscoverItem(inspectable.itemName);
    }

    public void CloseInspection()
    {
        if (currentItem != null)
            Destroy(currentItem);

        inspectionCanvas.SetActive(false);
    }

    private void LateUpdate()
    {
        if (inspectionCanvas.activeSelf && cam != null)
        {
            Vector3 directionToPlayer = inspectionCanvas.transform.position - cam.transform.position;
            directionToPlayer.y = 0;

            if (directionToPlayer.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer.normalized, Vector3.up);
                inspectionCanvas.transform.rotation = targetRotation;
            }
        }
    }

    private void ShowFullDescription()
    {
        shortDescriptionPanel.SetActive(false);
        fullDescriptionPanel.SetActive(true);
        backButton.gameObject.SetActive(true);
        readMoreButton.gameObject.SetActive(false);
        itemImageObject.SetActive(false);
    }

    private void ShowShortDescription()
    {
        shortDescriptionPanel.SetActive(true);
        fullDescriptionPanel.SetActive(false);
        backButton.gameObject.SetActive(false);
        readMoreButton.gameObject.SetActive(true);
        itemImageObject.SetActive(true);
    }
}
