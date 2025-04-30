using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to load scenes from onclick events in buttons
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [Header("Basic Data")]
    public string sceneName;
    public bool isAsync = true;
    public LoadSceneMode loadSceneMode = LoadSceneMode.Single;

    // When collider exists
    [Header("When Trigger Collider Exists")]
    public SpawnLocation spawnLocation = null;

    /// <summary>
    /// If the player enters the trigger, load the specified <see cref="sceneName"/>
    /// </summary>
    /// <param name="collision">Collider of the parent gameObject</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerManager.Instance.spawnLocation = spawnLocation;
            // transition here or in sceneController?
            LoadScene();
        }
    }

    public void LoadScene()
    {
        SceneController.Instance.LoadScene(sceneName, isAsync, loadSceneMode);
    }
}
