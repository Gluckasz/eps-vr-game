using System;
using UnityEngine;

public class TurnToCamera : MonoBehaviour
{
    private GameObject mainCamera;

    public string mainCameraTag = "MainCamera";

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag(mainCameraTag);

        if (mainCamera == null)
        {
            Debug.LogError(
                $"Main camera with tag '{mainCameraTag}' not found. Please ensure the camera exists and is tagged correctly."
            );
        }
    }

    private void Update()
    {
        transform.LookAt(mainCamera.transform);
        Vector3 newRotation = transform.rotation.eulerAngles;
        newRotation.x = 0;
        newRotation.y -= 180;
        newRotation.z = 0;

        transform.rotation = Quaternion.Euler(newRotation);
    }
}
