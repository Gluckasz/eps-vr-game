using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Rigidbody))]
public class DoorSound : MonoBehaviour
{
    public float openAngleThreshold = 5f;
    public float velocityThreshold = 0.1f;
    public float cooldown = 1.5f;
    public AudioClip[] openSounds;

    private Rigidbody rb;
    private AudioSource audioSource;
    private float lastPlayTime = -1f;
    private bool isDoorOpen = false;
    private bool wasMoving = false;
    private Quaternion initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        initialRotation = transform.rotation;
    }

    void Update()
    {
        bool isMovingNow = rb.angularVelocity.magnitude > velocityThreshold;

        float currentAngle = Quaternion.Angle(transform.rotation, initialRotation);

        bool isOpenNow = currentAngle > openAngleThreshold;
        if (wasMoving && !isMovingNow)
        {
            if (isOpenNow != isDoorOpen)
            {
                if (Time.time > lastPlayTime + cooldown)
                {
                    if (isOpenNow)
                    {
                        PlayRandomSound(openSounds);
                    }
                    lastPlayTime = Time.time;
                }

                isDoorOpen = isOpenNow;
            }
        }

        wasMoving = isMovingNow;
    }

    void PlayRandomSound(AudioClip[] sounds)
    {
        if (sounds == null || sounds.Length == 0) return;

        int randomIndex = Random.Range(0, sounds.Length);
        audioSource.clip = sounds[randomIndex];
        audioSource.Play();
    }
}
