using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Casters;
using UnityEngine.XR.Interaction.Toolkit.Interactors.Visuals;

public class LimitedDistanceRayInteractor : MonoBehaviour
{
    public NearFarInteractor nearFarInteractor;
    public float maxRayDistance = 3f; // Set your desired max distance

    private void Start()
    {
        if (nearFarInteractor == null)
            nearFarInteractor = GetComponent<NearFarInteractor>();

        if (nearFarInteractor != null)
        {
            // Get the CurveInteractionCaster through reflection
            var curveInteractionCaster = nearFarInteractor.farInteractionCaster;

            // Try to find a field or property for max distance
            var curveType = curveInteractionCaster.GetType();

            // Look for common properties that might control the ray distance
            // Examples include: maxRaycastDistance, rayLength, maxLength, etc.

            // Try to find a component on the same GameObject that controls ray length
            var lineVisual = GetComponent<XRInteractorLineVisual>();
            if (lineVisual != null)
            {
                // If the line visual component is present, we can limit the line length
                lineVisual.lineLength = maxRayDistance;
            }
        }
    }
}