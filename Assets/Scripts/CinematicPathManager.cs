using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRWalkingPath : MonoBehaviour
{
    public GameObject xrRigObject; // Assign your XR Rig here
    public Transform[] pathPoints; // Add your path waypoints here in the inspector
    public float moveSpeed = 1.0f; // Speed of walking
    public float bobFrequency = 1.5f; // Bobbing cycles per second
    public float bobAmplitude = 0.05f; // Bob height
    public AudioSource footstepSource; // Audio source for footstep sounds
    public AudioClip footstepClip; // Footstep sound effect

    private int currentTargetIndex = 0;
    private Transform xrRig;
    private bool isMoving = true;
    private float bobTimer = 0f;
    private bool playedStepThisBob = false;

    void Start()
    {
        xrRig = xrRigObject.transform;

        if (pathPoints.Length == 0)
        {
            Debug.LogError("No path points assigned!");
            enabled = false;
            return;
        }

        // Start at the first point
        xrRig.position = pathPoints[0].position;
        currentTargetIndex = 1; // Next point
    }

    void Update()
    {
        if (!isMoving || currentTargetIndex >= pathPoints.Length)
            return;

        Transform target = pathPoints[currentTargetIndex];

        // Move toward the next point
        Vector3 direction = (target.position - xrRig.position).normalized;
        Vector3 move = direction * moveSpeed * Time.deltaTime;
        xrRig.position += move;

        // Rotate rig to face the direction
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            float angleDiff = Quaternion.Angle(xrRig.rotation, toRotation);
            float rotationSpeed = Mathf.Clamp(angleDiff / 45f, 0.1f, 1f); // Slower turn if angle is sharp
            xrRig.rotation = Quaternion.Slerp(xrRig.rotation, toRotation, Time.deltaTime * 2f * rotationSpeed);

        }

        // Check if close enough to go to next point
        if (Vector3.Distance(xrRig.position, target.position) < 0.1f)
        {
            currentTargetIndex++;
        }

        // Add head bobbing effect
        bobTimer += Time.deltaTime * bobFrequency * Mathf.PI * 2f;
        float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;
        xrRig.position += Vector3.up * bobOffset;

        // Play footstep sound at bottom of bob
        if (Mathf.Sin(bobTimer) < -0.95f && !playedStepThisBob)
        {
            if (footstepSource != null && footstepClip != null)
            {
                footstepSource.PlayOneShot(footstepClip);
            }
            playedStepThisBob = true;
        }
        else if (Mathf.Sin(bobTimer) > -0.5f)
        {
            playedStepThisBob = false;
        }
    }

    // Call this to pause or resume walking
    public void SetWalking(bool walking)
    {
        isMoving = walking;
    }
}
