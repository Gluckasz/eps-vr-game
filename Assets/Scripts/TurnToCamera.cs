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
        Vector3 cameraEulerAngles = mainCamera.transform.rotation.eulerAngles;

        Vector3 newRotationEuler = new Vector3(cameraEulerAngles.x, cameraEulerAngles.y, 0f);

        transform.rotation = Quaternion.Euler(newRotationEuler);
    }
}
