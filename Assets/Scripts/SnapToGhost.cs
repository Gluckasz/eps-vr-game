using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SnapToGhost : MonoBehaviour
{
    public enum SnapType { Glass, Dish }

    [Header("Ghost Blueprint Settings")]
    public SnapType type;
    public static List<Transform> allGhostsGlass = new List<Transform>();
    public static List<Transform> allGhostsDish = new List<Transform>();

    [Header("Snapping Settings")]
    public float snapDistance = 0.2f;

    [Header("Highlight Settings")]
    public Color highlightColor = Color.cyan;
    public Color warningColor = Color.red;

    private Material ghostOriginalMaterial;
    private Renderer ghostRenderer;
    private XRGrabInteractable grabInteractable;

    public bool isSnapped = false;
    private Transform closestGhost = null;

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
        if (isSnapped) return;

        closestGhost = FindClosestGhost();

        if (closestGhost != null)
        {
            ghostRenderer = closestGhost.GetComponent<Renderer>();
            if (ghostRenderer != null)
            {
                ghostOriginalMaterial = new Material(ghostRenderer.material);
                ghostRenderer.material.color = highlightColor;
            }
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (isSnapped) return;

        Transform closestGhost = FindClosestGhost();
        if (closestGhost == null) return;

        float distance = Vector3.Distance(transform.position, closestGhost.position);
        if (distance <= snapDistance)
        {
            // Reset attach transform offsets
            if (grabInteractable.attachTransform != null)
            {
                grabInteractable.attachTransform.localPosition = Vector3.zero;
                grabInteractable.attachTransform.localRotation = Quaternion.identity;
            }

            // Snap to position and rotation
            transform.SetPositionAndRotation(closestGhost.position, closestGhost.rotation);

            // Remove ghost
            Destroy(closestGhost.gameObject);

            // Disable grabbing
            grabInteractable.enabled = false;

            // Disable Rigidbody physics
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;     // Makes it unaffected by physics
                rb.useGravity = false;     // Optional: ensure it doesn’t fall
                rb.constraints = RigidbodyConstraints.FreezeAll; // Literally freeze all motion/rotation
            }

            // Reset highlight
            if (ghostRenderer != null && ghostOriginalMaterial != null)
            {
                ghostRenderer.material = ghostOriginalMaterial;
            }

            isSnapped = true;
            SnapCountManager.Instance.totalSnapCount++;

            // Clear lock
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
}
