using UnityEngine;

public class SnapCountManager : MonoBehaviour
{
    private bool startedSceneDialogue = false;

    public int totalSnapCount = 8;
    public int CurrentSnapCount { get; set; } = 0;
    public static SnapCountManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (CurrentSnapCount >= totalSnapCount && !startedSceneDialogue)
        {
            startedSceneDialogue = true;
            Debug.Log("Starting scene dialogue.");
            SceneFlowManager.Instance.ShowDialogueInquiry();
        }
    }
}
