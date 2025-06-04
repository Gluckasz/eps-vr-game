using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SnapToGhost : MonoBehaviour
{
    public enum SnapType
    {
        Glass,
        Dish,
    }

    [Header("Ghost Blueprint Settings")]
    public SnapType type;
    public static List<Transform> allGhostsGlass = new List<Transform>();
    public static List<Transform> allGhostsDish = new List<Transform>();

    [Header("Snapping Settings")]
    public float snapDistance = 0.2f;
    public AudioSource itemPutSound;
    public AudioSource allItemsPutSound;

    [Header("Highlight Settings")]
    public Color highlightColor = Color.cyan;
    public Color warningColor = Color.red;

    private Material ghostOriginalMaterial;
    private Renderer ghostRenderer;
    private XRGrabInteractable grabInteractable;

    public bool isSnapped = false;
    private Transform closestGhost = null;
    private bool isBeingHeld = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnRelease);
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnDestroy()
    {
        grabInteractable.selectExited.RemoveListener(OnRelease);
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isBeingHeld = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isBeingHeld = false;
        TrySnapToGhost(); // Try snapping
        RemoveHighlight(); // Always remove ghost highlight when released
    }

    private void Update()
    {
        if (!isBeingHeld || isSnapped)
            return;

        Transform newClosestGhost = FindClosestGhost();

        if (newClosestGhost != closestGhost)
        {
            RemoveHighlight();
            closestGhost = newClosestGhost;
            ApplyHighlight();
        }
    }

    private void TrySnapToGhost()
    {
        if (isSnapped || closestGhost == null)
            return;

        float distance = Vector3.Distance(transform.position, closestGhost.position);
        if (distance <= snapDistance)
        {
            // Reset attach transform offsets
            if (grabInteractable.attachTransform != null)
            {
                grabInteractable.attachTransform.localPosition = Vector3.zero;
                grabInteractable.attachTransform.localRotation = Quaternion.identity;
            }

            // Snap to ghost position/rotation
            transform.SetPositionAndRotation(closestGhost.position, closestGhost.rotation);

            // Destroy ghost
            Destroy(closestGhost.gameObject);

            // Disable grabbing
            grabInteractable.enabled = false;

            // Freeze Rigidbody
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

            RemoveHighlight();

            isSnapped = true;
            SnapCountManager.Instance.CurrentSnapCount++;
            itemPutSound?.Play();

            if (SnapCountManager.Instance.CurrentSnapCount == SnapCountManager.Instance.totalSnapCount)
            {
                allItemsPutSound?.Play();
            }

            if (SnapManager.lockedCup == this)
            {
                SnapManager.lockedCup = null;
            }
        }
    }

    private Transform FindClosestGhost()
    {
        List<Transform> relevantList = type == SnapType.Glass ? allGhostsGlass : allGhostsDish;

        Transform closest = null;
        float minDistance = float.MaxValue;

        foreach (var ghost in relevantList)
        {
            float dist = Vector3.Distance(transform.position, ghost.position);
            if (dist < minDistance)
            {
                closest = ghost;
                minDistance = dist;
            }
        }

        return closest;
    }

    private void ApplyHighlight()
    {
        if (closestGhost == null)
            return;

        ghostRenderer = closestGhost.GetComponent<Renderer>();

        if (ghostRenderer != null)
        {
            ghostOriginalMaterial = new Material(ghostRenderer.material);
            ghostRenderer.material.color = highlightColor;
        }
    }

    private void RemoveHighlight()
    {
        if (ghostRenderer != null && ghostOriginalMaterial != null)
        {
            ghostRenderer.material = ghostOriginalMaterial;
        }

        ghostRenderer = null;
        ghostOriginalMaterial = null;
        closestGhost = null;
    }
}
