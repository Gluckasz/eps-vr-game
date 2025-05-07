using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SnapToGhost : MonoBehaviour
{
    [Header("Ghost Blueprint Settings")]
    public Transform ghostTransform;

    [Header("Snapping Settings")]
    public float snapDistance = 0.2f;

    [Header("Highlight Settings")]
    public Color highlightColor = Color.cyan;
    private Material ghostOriginalMaterial;
    private Renderer ghostRenderer;

    private XRGrabInteractable grabInteractable;
    private bool isSnapped = false;

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

        if (ghostTransform != null)
        {
            ghostRenderer = ghostTransform.GetComponent<Renderer>();
            if (ghostRenderer != null)
            {
                ghostOriginalMaterial = new Material(ghostRenderer.material);
                ghostRenderer.material.color = highlightColor;
            }

            //var interactor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor;
            //interactor?.SendHapticImpulse(0.5f, 0.1f);
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (isSnapped || ghostTransform == null) return;

        float distance = Vector3.Distance(transform.position, ghostTransform.position);

        if (distance <= snapDistance)
        {
            // Reset attach transform offsets to avoid weird rotations
            if (grabInteractable.attachTransform != null)
            {
                grabInteractable.attachTransform.localPosition = Vector3.zero;
                grabInteractable.attachTransform.localRotation = Quaternion.identity;
            }

            // Snap position and rotation
            transform.SetPositionAndRotation(ghostTransform.position, ghostTransform.rotation);

            // Disable grabbing
            grabInteractable.enabled = false;

            // Destroy or hide ghost
            Destroy(ghostTransform.gameObject);

            // Reset highlight if any
            if (ghostRenderer != null && ghostOriginalMaterial != null)
                ghostRenderer.material = ghostOriginalMaterial;

            isSnapped = true;
        }
    }
}