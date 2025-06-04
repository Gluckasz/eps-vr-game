using System.Collections.Generic;
using UnityEngine;

public class ObjectPusher : MonoBehaviour
{
    public float launchForce = 10f;
    public Vector3 localLaunchDirection = new Vector3(0, 0, 1);

    void OnTriggerStay(Collider other)
    {
        Rigidbody targetRigidbody = other.attachedRigidbody;

        if (
            targetRigidbody != null
            && !targetRigidbody.isKinematic
            && targetRigidbody.gameObject.tag != "UI"
        )
        {
            Vector3 worldLaunchDirection = transform.TransformDirection(
                localLaunchDirection.normalized
            );

            targetRigidbody.AddForce(
                worldLaunchDirection * launchForce,
                ForceMode.Force
            );
            Debug.Log(
                gameObject.name
                    + " launched "
                    + other.name
                    + " (while staying in trigger). Applied force: "
                    + (worldLaunchDirection * launchForce)
            );
        }
    }
}
