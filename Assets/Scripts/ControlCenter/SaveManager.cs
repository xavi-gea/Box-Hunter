using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage the save data by saving or loading it
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public const string saveFileName = "SaveData.json";

    public bool isLoadingFromSave = false;

    private void Awake()
    {
        Instance = this;

        LoadPlayerPrefs();
    }

    public void SavePlayerPrefs()
    {
        if (Instance == this)
        {
            PlayerPrefs.SetFloat("AudioMainVolume", AudioListener.volume);
        }
        else
        {
            Instance.SavePlayerPrefs();
        }
    }

    private void LoadPlayerPrefs()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("AudioMainVolume");
    }

    /// <summary>
    /// Save relevant game data to a persistent JSON file by using the content of an instantiated <see cref="SaveData"/>
    /// </summary>
    public void Save()
    {
        if (Instance == this)
        {
            SaveData saveData = new();
            saveData.currentSceneName = SceneManager.GetActiveScene().name;
            saveData.playerPosition = PlayerManager.Instance.playerGameObject.transform.position;

            string dialogueContent = "Partida guardada";

            if (!FileManager.WriteToFile(saveFileName, JsonUtility.ToJson(saveData)))
            {
                dialogueContent = "Error al guardar la partida";
            }

            Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();
            dialogue.dialogueLines = new string[] { dialogueContent };

            DialogueManager.Instance.StartDialogue(dialogue);
        }
        else
        {
            Instance.Save();
        }
    }


    /// <summary>
    /// Try to load existing saved data from a file, put it in a instantiated <see cref="SaveData"/> and use it
    /// </summary>
    public void Load()
    {
        if (Instance == this)
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(FileManager.LoadFromFile(saveFileName));

            if (saveData != null)
            {
                SpawnLocation saveSpawnLocation = ScriptableObject.CreateInstance<SpawnLocation>();
                saveSpawnLocation.position = saveData.playerPosition;

                // first playerGameObject
                PlayerManager.Instance.saveSpawnLocation = saveSpawnLocation;

                isLoadingFromSave = true;

                // then currentSceneName
                SceneController.Instance.LoadScene(saveData.currentSceneName,true,LoadSceneMode.Single);
            }
        }
        else
        {
            Instance.Load();
        }
    }
}
