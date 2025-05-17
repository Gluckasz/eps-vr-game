using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Rigidbody))]
public class DoorSound : MonoBehaviour
{
    public float movementThreshold = 0.1f;
    public float cooldown = 0.5f;
    public AudioClip[] doorSounds; // Assign your sound clips in the Inspector

    private Rigidbody rb;
    private AudioSource audioSource;
    private float lastPlayTime = -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (rb.angularVelocity.magnitude > movementThreshold)
        {
            if (!audioSource.isPlaying && Time.time > lastPlayTime + cooldown)
            {
                PlayRandomSound();
                lastPlayTime = Time.time;
            }
        }
    }

    void PlayRandomSound()
    {
        if (doorSounds.Length == 0) return;

        int randomIndex = Random.Range(0, doorSounds.Length);
        audioSource.clip = doorSounds[randomIndex];
        audioSource.Play();
    }
}
