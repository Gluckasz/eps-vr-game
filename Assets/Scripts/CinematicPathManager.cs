using UnityEngine;

public class VRCinematicPath : MonoBehaviour
{
    public Transform[] pathPoints;
    public Transform xrRig;
    public float moveDuration = 10f; // total time to move across path
    public bool playOnStart = true;

    private float timer = 0f;
    private int segmentIndex = 0;
    private bool isPlaying = false;

    void Start()
    {
        if (playOnStart)
        {
            StartCinematic();
        }
    }

    public void StartCinematic()
    {
        if (pathPoints.Length < 2 || xrRig == null)
        {
            Debug.LogWarning("Missing waypoints or XR Rig!");
            return;
        }

        segmentIndex = 0;
        timer = 0f;
        isPlaying = true;
    }

    void Update()
    {
        if (!isPlaying || segmentIndex + 1 >= pathPoints.Length) return;

        float segmentTime = moveDuration / (pathPoints.Length - 1);
        timer += Time.deltaTime;
        float t = timer / segmentTime;

        xrRig.position = Vector3.Lerp(
            pathPoints[segmentIndex].position,
            pathPoints[segmentIndex + 1].position,
            t
        );

        if (t >= 1f)
        {
            segmentIndex++;
            timer = 0f;
        }

        // Stop at the end
        if (segmentIndex + 1 >= pathPoints.Length)
        {
            isPlaying = false;
            Debug.Log("Cinematic ended.");
        }
    }
}
