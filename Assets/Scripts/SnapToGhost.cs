using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SnapToGhost : MonoBehaviour
{
    [Header("Ghost Blueprint Settings")]
    public Transform ghostTransform;

    [Header("Snapping Settings")]
    public float snapDistance = 0.2f;

    [Header("Highlight Settings")]
    public Color highlightColor = Color.cyan;
    public Color warningColor = Color.red;

    private Material ghostOriginalMaterial;
    private Renderer ghostRenderer;

    private XRGrabInteractable grabInteractable;
    public bool isSnapped = false;

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
        // Check if a cup is already locked and not yet snapped
        if (SnapManager.lockedCup != null && SnapManager.lockedCup != this && !SnapManager.lockedCup.isSnapped)
        {
            // Highlight the correct ghost
            var otherGhost = SnapManager.lockedCup.ghostTransform?.GetComponent<Renderer>();
            if (otherGhost != null)
            {
                otherGhost.material.color = warningColor;
            }

            // Cancel grabbing this one
            if (args.interactorObject is IXRSelectInteractor interactor)
            {
                var interactionManager = grabInteractable.interactionManager;
                interactionManager?.CancelInteractableSelection(grabInteractable as IXRSelectInteractable);
            }

            return;
        }

        // Lock this as the one to place
        if (SnapManager.lockedCup == null)
        {
            SnapManager.lockedCup = this;
        }

        if (isSnapped) return;

        if (ghostTransform != null)
        {
            ghostRenderer = ghostTransform.GetComponent<Renderer>();
            if (ghostRenderer != null)
            {
                ghostOriginalMaterial = new Material(ghostRenderer.material);
                ghostRenderer.material.color = highlightColor;
            }
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (isSnapped || ghostTransform == null)
            return;

        float distance = Vector3.Distance(transform.position, ghostTransform.position);

        if (distance <= snapDistance)
        {
            // Reset attach transform offsets
            if (grabInteractable.attachTransform != null)
            {
                grabInteractable.attachTransform.localPosition = Vector3.zero;
                grabInteractable.attachTransform.localRotation = Quaternion.identity;
            }

            // Snap!
            transform.SetPositionAndRotation(ghostTransform.position, ghostTransform.rotation);

            // Disable grabbing
            grabInteractable.enabled = false;

            // Remove ghost
            Destroy(ghostTransform.gameObject);

            // Reset highlight
            if (ghostRenderer != null && ghostOriginalMaterial != null)
            {
                ghostRenderer.material = ghostOriginalMaterial;
            }

            isSnapped = true;

            // Clear lock so next glass can be picked
            if (SnapManager.lockedCup == this)
            {
                SnapManager.lockedCup = null;
            }
            SnapCountManager.Instance.CurrentSnapCount += 1;
        }
    }
}
