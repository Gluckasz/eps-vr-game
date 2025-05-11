using System;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private GameObject mainCamera;

    public string mainCameraTag = "MainCamera";
    public bool IsFollowingCamera { get; set; } = false;
    public Vector3 offset = new(0, -0.7f, 1.5f);
    public float movementThreshold = 0.008f;

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

        if (IsFollowingCamera)
        {
            Vector3 comparePosition = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z
            );
            transform.position = mainCamera.transform.position;
            transform.Translate(offset, mainCamera.transform);

            if (Math.Abs(comparePosition.x - transform.position.x) < movementThreshold)
            {
                transform.position = new Vector3(
                    comparePosition.x,
                    transform.position.y,
                    transform.position.z
                );
                Debug.Log(
                    "Moving back X Axis: "
                        + gameObject.name
                        + " From: "
                        + transform.position.x
                        + " To: "
                        + comparePosition.x
                );
            }
            if (Math.Abs(comparePosition.y - transform.position.y) < movementThreshold)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    comparePosition.y,
                    transform.position.z
                );
                Debug.Log(
                    "Moving back Y Axis: "
                        + gameObject.name
                        + " From: "
                        + transform.position.y
                        + " To: "
                        + comparePosition.y
                );
            }
            if (Math.Abs(comparePosition.z - transform.position.z) < movementThreshold)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    comparePosition.z
                );
                Debug.Log(
                    "Moving back Z Axis: "
                        + gameObject.name
                        + " From: "
                        + transform.position.z
                        + " To: "
                        + comparePosition.z
                );
            }
        }
    }
}
