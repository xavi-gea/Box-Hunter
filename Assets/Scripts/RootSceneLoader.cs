using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Verify that the Root scene has been previously loaded
/// </summary>
public class RootSceneLoader : MonoBehaviour
{
    public string rootSceneName = "Root";
    public string controlCenterTagName = "GameController";

    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag(controlCenterTagName).Length == 0)
        {
            SceneManager.LoadScene(rootSceneName, LoadSceneMode.Additive);
        }
    }
}
