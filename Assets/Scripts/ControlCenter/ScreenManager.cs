using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [Header("Pause")]
    public bool isGamePaused = false;
    public GameObject pauseMenuPrefab;
    private GameObject instantiatedPauseMenu;

    [Header("Main Menu")]
    public string mainMenuSceneName;

    /// <summary>
    /// Currently Open Screen
    /// </summary>
    private GameObject openedScreen;

    /// <summary>
    /// The GameObject Selected before we opened the current Screen.
    /// Used when closing a Screen, so we can go back to the button that opened it.
    /// </summary>
    private GameObject previouslySelected;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Closes the currently open panel and opens the provided one.
    /// It also takes care of handling the navigation, setting the new Selected element.
    /// </summary>
    /// <param name="screenToOpen"></param>
    public void OpenPanel(GameObject screenToOpen)
    {
        if (Instance == this)
        {
            if (openedScreen == screenToOpen)
                return;

            //Activate the new Screen hierarchy
            screenToOpen.SetActive(true);
            //Save the currently selected button that was used to open this Screen. (CloseCurrent will modify it)
            var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;
            //Move the Screen to front.
            screenToOpen.transform.SetAsLastSibling();

            CloseCurrent();

            previouslySelected = newPreviouslySelected;

            //Set the new Screen as the opened one.
            openedScreen = screenToOpen;

            //Set an element in the new screen as the new Selected one.
            SetSelected(FindFirstEnabledSelectable(screenToOpen));
        }
        else
        {
            Instance.OpenPanel(screenToOpen);
        }
    }

    /// <summary>
    /// Finds the first Selectable element in the providade hierarchy.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    private static GameObject FindFirstEnabledSelectable(GameObject gameObject)
    {
        GameObject go = null;
        var selectables = gameObject.GetComponentsInChildren<Selectable>(true);
        foreach (var selectable in selectables)
        {
            if (selectable.IsActive() && selectable.IsInteractable())
            {
                go = selectable.gameObject;
                break;
            }
        }
        return go;
    }

    /// <summary>
    /// Closes the currently open Screen.
    /// It also takes care of navigation.
    /// Reverting selection to the Selectable used before opening the current screen.
    /// </summary>
    public void CloseCurrent()
    {
        if (Instance == this)
        {
            if (openedScreen == null)
                return;

            //Reverting selection to the Selectable used before opening the current screen.
            SetSelected(previouslySelected);

            openedScreen.SetActive(false);

            //No screen open.
            openedScreen = null;
        }
        else
        {
            Instance.CloseCurrent();
        }
    }

    /// <summary>
    /// Make the provided GameObject selected
    /// </summary>
    /// <param name="targetGameObject"></param>
    private void SetSelected(GameObject targetGameObject)
    {
        EventSystem.current.SetSelectedGameObject(targetGameObject);
    }

    /// <summary>
    /// Unpause the game
    /// </summary>
    public void UnPauseGame()
    {
        if (Instance == this)
        {
            // close current or close current and unpause depeding how deep?
            // could be circumvented if I always add a "go back" button

            Destroy(instantiatedPauseMenu);
            InputManager.Instance.SetInputMap(ActionMap.Player);
            CloseCurrent();
            isGamePaused = false;
        }
        else
        {
            Instance.UnPauseGame();
        }
    }

    /// <summary>
    /// Pause the game
    /// </summary>
    public void PauseGame()
    {
        if (Instance == this)
        {
            instantiatedPauseMenu = Instantiate(pauseMenuPrefab, GameObject.Find("Canvas").transform);

            SetSelected(FindFirstEnabledSelectable(instantiatedPauseMenu));
            InputManager.Instance.SetInputMap(ActionMap.UI);
            isGamePaused = true;
        }
        else
        {
            Instance.PauseGame();
        }
    }

    /// <summary>
    /// Open the main menu scene
    /// </summary>
    public void GoToMainMenu()
    {
        UnPauseGame();
        InputManager.Instance.SetInputMap(ActionMap.UI);

        SceneManager.LoadSceneAsync(mainMenuSceneName);
    }

    /// <summary>
    /// Close the game or unity editor
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
#else
        Application.Quit();
#endif
    }
}
