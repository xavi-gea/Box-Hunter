using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAtStart : MonoBehaviour
{
    [Tooltip("Name of the scene that will be loaded. Must be in the build index. Case insensitive")]
    public string sceneName;

    void Start()
    {
        if (!SceneManager.GetActiveScene().name.Equals(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
