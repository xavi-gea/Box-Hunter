using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public const string saveFileName = "SaveData.json";

    private void Awake()
    {
        Instance = this;
    }

    public void Save()
    {
        if (Instance == this)
        {
            SaveData saveData = new();
            saveData.currentSceneName = SceneManager.GetActiveScene().name;
            saveData.playerPosition = PlayerManager.Instance.playerGameObject.transform.position;

            //FileManager.WriteToFile(saveFileName, JsonUtility.ToJson(saveData),"SaveData");

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
