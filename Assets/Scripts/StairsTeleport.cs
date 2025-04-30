using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRStairTeleport : MonoBehaviour
{
    public Transform teleportTarget;
    public float teleportCooldown = 1f;
    private bool canTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        XROrigin xrOrigin = other.GetComponentInParent<XROrigin>();
        if (xrOrigin != null && canTeleport)
        {
            StartCoroutine(TeleportRoutine(xrOrigin));
        }
    }

    private System.Collections.IEnumerator TeleportRoutine(XROrigin xrOrigin)
    {
        canTeleport = false;

        // Wait 1 frame to avoid physics conflict
        yield return null;

        // Get camera offset (where headset is)
        Transform cam = xrOrigin.Camera.transform;
        Vector3 cameraOffset = cam.position - xrOrigin.transform.position;

        // Calculate new position so camera ends up at target
        Vector3 newPosition = teleportTarget.position - cameraOffset;

        // Move the entire rig
        xrOrigin.transform.position = newPosition;

        // Optional: small wait before allowing another teleport
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }
}
