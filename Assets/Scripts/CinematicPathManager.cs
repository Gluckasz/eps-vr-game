using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRWalkingPath : MonoBehaviour
{
    public GameObject xrRigObject; // Assign your XR Rig here
    public Transform[] pathPoints; // Add your path waypoints here in the inspector
    public float moveSpeed = 1.0f; // Speed of walking
    public float bobFrequency = 0.5f; // Bobbing cycles per second
    public float bobAmplitude = 0.001f; // Bob height
    public AudioSource footstepSource; // Audio source for footstep sounds
    public AudioClip footstepClip; // Looping footstep sound

    private int currentTargetIndex = 0;
    private Transform xrRig;
    private bool isMoving = true;
    private float bobTimer = 0f;

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
        currentTargetIndex = 1;

        // Set up audio looping
        if (footstepSource != null && footstepClip != null)
        {
            footstepSource.clip = footstepClip;
            footstepSource.loop = true;
        }
    }

    void Update()
    {
        if (currentTargetIndex >= pathPoints.Length)
        {
            if (isMoving)
            {
                isMoving = false;
                if (footstepSource.isPlaying)
                    footstepSource.Stop();
            }
            return;
        }

        if (!isMoving)
            return;

        Transform target = pathPoints[currentTargetIndex];

        // Move toward the next point
        Vector3 direction = (target.position - xrRig.position).normalized;
        Vector3 move = direction * moveSpeed * Time.deltaTime;
        xrRig.position += move;

        // Smooth turning
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            xrRig.rotation = Quaternion.Slerp(xrRig.rotation, toRotation, Time.deltaTime * 2.5f); // slower turn for more human-like motion
        }

        // Start looping footstep if not playing
        if (footstepSource != null && !footstepSource.isPlaying)
        {
            footstepSource.Play();
        }

        // Check if close enough to go to next point
        if (Vector3.Distance(xrRig.position, target.position) < 0.1f)
        {
            currentTargetIndex++;
        }

        // Head bobbing
        bobTimer += Time.deltaTime * bobFrequency * Mathf.PI * 2f;
        float bobOffset = Mathf.Sin(bobTimer) * bobAmplitude;
        xrRig.position += Vector3.up * bobOffset;
    }

    public void SetWalking(bool walking)
    {
        isMoving = walking;

        if (!walking && footstepSource != null && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }

        if (walking && footstepSource != null && !footstepSource.isPlaying)
        {
            footstepSource.Play();
        }
    }
}
