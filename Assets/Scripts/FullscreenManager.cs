using TMPro;
using UnityEngine;

public class FullscreenManager : MonoBehaviour
{
    private TextMeshProUGUI buttonText;

    private int windowedWidth = Screen.width;
    private int windowedHeight = Screen.height;

    private Vector2Int windowedPosition;

    private readonly string fullScreenText = "Pantalla completa";
    private readonly string windowedText = "Ventana";

    private void Awake()
    {
        buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        if (Screen.fullScreen)
        {
            UpdateButtonText(fullScreenText);
        }
        else
        {
            UpdateButtonText(windowedText);
        }

    }
    public void ChangeScreenMode()
    {
        if (Screen.fullScreen)
        {
            Screen.SetResolution(width: windowedWidth, height: windowedHeight, false);
            Screen.MoveMainWindowTo(Screen.mainWindowDisplayInfo, windowedPosition);

            UpdateButtonText(windowedText);
        }
        else
        {
            windowedWidth = Screen.width;
            windowedHeight = Screen.height;
            windowedPosition = Screen.mainWindowPosition;

            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

            UpdateButtonText(fullScreenText);
        }
    }

    private void UpdateButtonText(string newText)
    {
        if (buttonText == null)
        {
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        buttonText.text = "Modo: " + newText;
    }
}


