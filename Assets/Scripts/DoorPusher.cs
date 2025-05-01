using UnityEngine;

public class DoorPusher : MonoBehaviour
{
    public float pushForce = 5f;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody doorRigidbody = hit.collider.attachedRigidbody;

        if (doorRigidbody != null && !doorRigidbody.isKinematic)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            doorRigidbody.AddForceAtPosition(pushDir * pushForce, hit.point);

            Debug.Log("Pushing door with force: " + (pushDir * pushForce));
        }
    }
}