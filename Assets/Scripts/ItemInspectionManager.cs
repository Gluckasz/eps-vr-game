using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.UI;

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

    private GameObject currentItem;
    private Camera cam;
    private Transform canvasTargetPosition;

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
            Debug.LogError("Missing InspectableItem component on: " + item.name);
            return;
        }

        item.SetActive(false);

        // Set UI content
        itemName.text = inspectable.itemName;
        descriptionText.text = inspectable.description;
        FulldescriptionText.text = inspectable.fullDescription;

        itemImageUI.enabled = inspectable.itemImage != null;
        if (inspectable.itemImage != null)
            itemImageUI.sprite = inspectable.itemImage;

        canvasTargetPosition = inspectable.itemPosition;

        inspectionCanvas.transform.position = canvasTargetPosition.position;

        ShowShortDescription();
        inspectionCanvas.SetActive(true);

        currentItem = Instantiate(item, itemHolder.position, itemHolder.rotation, itemHolder);
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb) { rb.isKinematic = true; rb.detectCollisions = false; }

        XRGrabInteractable grab = currentItem.GetComponent<XRGrabInteractable>();
        if (grab) grab.enabled = false;

        foreach (var col in currentItem.GetComponentsInChildren<Collider>())
            col.enabled = false;

        ItemDiscoveryManager.instance.DiscoverItem(inspectable.itemName);

        LazyFollow lazy = inspectionCanvas.GetComponent<LazyFollow>();
        if (lazy != null)
        {
            lazy.rotationFollowMode = LazyFollow.RotationFollowMode.None;
        }
    }

    public void CloseInspection()
    {
        if (currentItem != null)
            Destroy(currentItem);

        inspectionCanvas.SetActive(false);
    }

    private void LateUpdate()
    {
        if (inspectionCanvas.activeSelf && cam != null && canvasTargetPosition != null)
        {
            inspectionCanvas.transform.position = canvasTargetPosition.position;

            Vector3 headPos = cam.transform.position;
            Vector3 desiredPos = inspectionCanvas.transform.position;

            Vector3 flatLookDir = (desiredPos - headPos); 
            flatLookDir.y = 0;

            if (flatLookDir.sqrMagnitude > 0.01f)
            {
                inspectionCanvas.transform.rotation = Quaternion.LookRotation(flatLookDir);
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