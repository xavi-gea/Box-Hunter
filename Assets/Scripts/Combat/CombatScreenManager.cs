using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatScreenManager : MonoBehaviour
{
    /// <summary>
    /// Currently Open Screen
    /// </summary>
    private GameObject openedScreen;

    /// <summary>
    /// The GameObject Selected before we opened the current Screen
    /// Used when closing a Screen, so we can go back to the button that opened it
    /// </summary>
    private GameObject previouslySelected;

    /// <summary>
    /// Closes the currently open panel and opens the provided one
    /// It also takes care of handling the navigation, setting the new Selected element
    /// </summary>
    /// <param name="screenToOpen"></param>
    public void OpenPanel(GameObject screenToOpen)
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
        GameObject go = FindFirstEnabledSelectable(screenToOpen);
        SetSelected(go);
    }

    /// <summary>
    /// Finds the first Selectable element in the providade hierarchy
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
    /// Closes the currently open Screen
    /// It also takes care of navigation
    /// Reverting selection to the Selectable used before opening the current screen
    /// </summary>
    public void CloseCurrent()
    {
        if (openedScreen == null)
            return;

        //Reverting selection to the Selectable used before opening the current screen.
        SetSelected(previouslySelected);

        openedScreen.SetActive(false);

        //No screen open.
        openedScreen = null;
    }

    /// <summary>
    /// Make the provided GameObject selected
    /// </summary>
    /// <param name="targetGameObject"></param>
    private void SetSelected(GameObject targetGameObject)
    {
        EventSystem.current.SetSelectedGameObject(targetGameObject);
    }
}