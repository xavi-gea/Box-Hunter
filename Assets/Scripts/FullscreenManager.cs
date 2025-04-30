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


    /// <summary>
    /// If the <see cref="Screen"/> is fullScreen, set the <see cref="buttonText"/> text as the <see cref="fullScreenText"/> one
    /// If not, use <see cref="windowedText"/>
    /// </summary>
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

    /// <summary>
    /// Toggle betweeen fullscreen and windowed, changing the <see cref="buttonText"/> text
    /// </summary>
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

    /// <summary>
    /// Change the <see cref="buttonText"/> text to the provided <paramref name="newText"/>
    /// </summary>
    /// <param name="newText"></param>
    private void UpdateButtonText(string newText)
    {
        if (buttonText == null)
        {
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        buttonText.text = "Modo: " + newText;
    }
}


