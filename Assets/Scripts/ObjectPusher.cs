using System.Collections.Generic;
using UnityEngine;

public class ObjectPusher : MonoBehaviour
{
    public float launchForce = 800f;
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
            float distance = 0;
            distance += Mathf.Abs(targetRigidbody.position.x - transform.position.x);
            distance += Mathf.Abs(targetRigidbody.position.z - transform.position.z);

            targetRigidbody.AddForce(
                worldLaunchDirection * launchForce / distance,
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
