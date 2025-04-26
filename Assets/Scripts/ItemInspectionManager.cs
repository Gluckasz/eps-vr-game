using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInspectionManager : MonoBehaviour
{
    public static ItemInspectionManager Instance;

    public GameObject inspectionCanvas; // assign in inspector
    public TextMeshProUGUI descriptionText; // assign in inspector
    public Button continueButton; // assign in inspector
    public Transform playerTransform; // assign player transform here in inspector

    private GameObject currentItem;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        inspectionCanvas.SetActive(false);
    }

    public void StartInspection(GameObject item)
    {
        currentItem = item;

        // Move item in front of player
        if (playerTransform != null)
        {
            Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 1.0f + Vector3.up * 0.5f;
            currentItem.transform.position = spawnPosition;
            currentItem.transform.rotation = Quaternion.LookRotation(playerTransform.forward);
        }

        currentItem.SetActive(true);

        inspectionCanvas.SetActive(true);

        descriptionText.text = "You found: " + currentItem.name + "!";

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(FinishInspection);
    }

    private void FinishInspection()
    {
        inspectionCanvas.SetActive(false);

        if (currentItem != null)
        {
            // You can here notify your inventory manager separately if you want!
            Debug.Log("Item added to inventory: " + currentItem.name);

            currentItem.SetActive(false);
        }

        currentItem = null;
    }
}
