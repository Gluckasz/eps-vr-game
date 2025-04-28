using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private const string SAVE_KEY = "GameSaveData";
    public bool HasSaveData => PlayerPrefs.HasKey(SAVE_KEY);

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
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveGame();
        }
    }

    public void SaveGame()
    {
        try
        {
            GameData data = CollectGameData();
            string jsonData = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SAVE_KEY, jsonData);
            PlayerPrefs.Save();
            Debug.Log("Game saved successfully using PlayerPrefs");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }

    public bool LoadGame()
    {
        if (!HasSaveData)
        {
            Debug.Log("No save data found");
            return false;
        }

        try
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            GameData data = JsonUtility.FromJson<GameData>(jsonData);
            ApplyGameData(data);
            Debug.Log("Game loaded successfully from PlayerPrefs");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
            return false;
        }
    }

    private GameData CollectGameData()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        GameData data = new GameData
        {
            playerPosition = player ? player.transform.position : Vector3.zero,
            playerRotation = player ? player.transform.rotation : Quaternion.identity,
            currentLevel = SceneManager.GetActiveScene().buildIndex,
        };

        return data;
    }

    private void ApplyGameData(GameData data)
    {
        if (SceneManager.GetActiveScene().buildIndex != data.currentLevel)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(data.currentLevel);
        }
        else
        {
            ApplyDataToScene(data);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameData data = LoadGameDataWithoutApplying();
        ApplyDataToScene(data);

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private GameData LoadGameDataWithoutApplying()
    {
        try
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                string jsonData = PlayerPrefs.GetString(SAVE_KEY);
                return JsonUtility.FromJson<GameData>(jsonData);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading save data: {e.Message}");
        }
        return new GameData();
    }

    private void ApplyDataToScene(GameData data)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            player.transform.position = data.playerPosition;
            player.transform.rotation = data.playerRotation;
        }
    }
}