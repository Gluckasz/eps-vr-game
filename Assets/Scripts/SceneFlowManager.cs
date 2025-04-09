using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class SceneFlowManager : MonoBehaviour
{
    private AudioSource audioSource;
    private bool introAudioPlayed = false;

    private DialogueDisplayManager dialogueDisplayManagerScript;

    public DialogueData dialogueData;
    public string scriptPath = "scene_1_script.json";
    public GameObject dialogueDisplayer;

    public static GameObject dialogueDisplayerInstance;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        dialogueDisplayerInstance = Instantiate(dialogueDisplayer);
        dialogueDisplayerInstance.SetActive(false);

        dialogueDisplayManagerScript = dialogueDisplayerInstance.GetComponent<DialogueDisplayManager>();

        LoadDialogueData();
    }

    private string ConstructDialogueText(DialogueNode node)
    {
        return node.character + ": " + node.text;
    }

    private void LoadDialogueData()
    {
        string filePath = Path.Combine(Application.dataPath, scriptPath);

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);

            dialogueData = JsonUtility.FromJson<DialogueData>(jsonContent);

            Debug.Log("Dialogue loaded successfully!");
            if (dialogueData.intro.Count > 0)
            {
                Debug.Log("Playing intro audio");
                PlayIntroAudio();
            }
        }
        else
        {
            Debug.LogError("Could not find JSON file at path: " + filePath);
        }
    }

    private void PlayIntroAudio()
    {
        string audioPath = dialogueData.intro[0].audio;

        string resourcePath = audioPath.Replace("Assets/", "").Replace(".wav", "");

        AudioClip introClip = Resources.Load<AudioClip>(resourcePath);

        if (introClip != null)
        {
            dialogueDisplayerInstance.SetActive(true);
            dialogueDisplayManagerScript.UpdateText(ConstructDialogueText(dialogueData.intro[0]));

            audioSource.clip = introClip;
            audioSource.Play();

            StartCoroutine(WaitForAudioToFinish(introClip.length));
        }
        else
        {
            Debug.LogError("Could not load audio clip from: " + resourcePath);
        }
    }

    private IEnumerator WaitForAudioToFinish(float delay)
    {
        yield return new WaitForSeconds(delay + 0.5f); // Small buffer after audio ends
        StartScene();
    }

    public void StartScene()
    {
        dialogueDisplayerInstance.SetActive(true);

        dialogueDisplayManagerScript.StartSceneDialogue(dialogueData);
    }
}
