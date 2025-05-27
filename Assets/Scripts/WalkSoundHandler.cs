using UnityEngine;

public class WalkingAudio : MonoBehaviour
{
    public AudioSource walkingAudio;

    private Vector3 lastPosition;
    private float movementThreshold = 0.01f;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        if (distanceMoved > movementThreshold)
        {
            if (!walkingAudio.isPlaying)
                walkingAudio.Play();
        }
        else
        {
            if (walkingAudio.isPlaying)
                walkingAudio.Pause();
        }

        lastPosition = transform.position;
    }
}
