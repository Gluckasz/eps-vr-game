using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance { get; private set; }
    private AudioSource musicAudioSource;

    public AudioClip dialogueMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        musicAudioSource = GetComponent<AudioSource>();
    }

    public void PlayDialogueMusic()
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = dialogueMusic;
        musicAudioSource.Play();
    }
}
