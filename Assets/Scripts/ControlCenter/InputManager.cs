using UnityEngine;
using UnityEngine.InputSystem;

public enum ActionMap
{
    Player,
    UI
}

/// <summary>
/// Manage player input
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private InputActionMap playerMap;
    private InputActionMap uiMap;

    /// <summary>
    /// Find and assign the player and UI <see cref="InputActionMap"/>
    /// </summary>
    private void Awake()
    {
        Instance = this;

        playerMap = InputSystem.actions.FindActionMap("Player");
        uiMap = InputSystem.actions.FindActionMap("UI");
    }

    /// <summary>
    /// By default, set the UI <see cref="ActionMap"/> as the active one
    /// </summary>
    private void Start()
    {
        SetInputMap(ActionMap.UI);
    }

    /// <summary>
    /// Set the specified <paramref name="actionMap"/> as the active one and disable the rest
    /// </summary>
    /// <param name="actionMap"></param>
    public void SetInputMap(ActionMap actionMap)
    {
        if (actionMap.Equals(ActionMap.Player))
        {
            playerMap.Enable();
            uiMap.Disable();
        }
        else if (actionMap.Equals(ActionMap.UI))
        {
            uiMap.Enable();
            playerMap.Disable();
        }
    }

    /// <summary>
    /// Enables the other <see cref="InputActionMap"/> and disables the current one
    /// </summary>
    public void ToggleInputMap()
    {
        if (playerMap.enabled)
        {
            uiMap.Enable();
            playerMap.Disable();
        }
        else
        {
            playerMap.Enable();
            uiMap.Disable();
        }
    }
}
