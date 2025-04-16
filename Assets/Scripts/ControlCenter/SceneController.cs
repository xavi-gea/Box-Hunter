using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void LoadScene(string sceneName, bool isAsync, LoadSceneMode loadSceneMode)
    {
        if (Instance == this)
        {
            if (sceneName.Equals(ScreenManager.Instance.mainMenuSceneName))
            {
                ScreenManager.Instance.UnPauseGame();
            }

            if (isAsync)
            {
                SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            }
            else
            {
                SceneManager.LoadScene(sceneName, loadSceneMode);
            }
        }
        else
        {
            Instance.LoadScene(sceneName, isAsync, loadSceneMode);
        }
    }
}
