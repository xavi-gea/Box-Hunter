using UnityEngine;
using UnityEngine.InputSystem;

public enum ActionMap
{
    Player,
    UI
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private InputActionMap playerMap;
    private InputActionMap uiMap;

    private void Awake()
    {
        Instance = this;

        playerMap = InputSystem.actions.FindActionMap("Player");
        uiMap = InputSystem.actions.FindActionMap("UI");
    }

    private void Start()
    {
        SetInputMap(ActionMap.UI);
    }

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
