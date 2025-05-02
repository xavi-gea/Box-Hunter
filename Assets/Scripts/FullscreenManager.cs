using TMPro;
using UnityEngine;

public class WindowedProperties
{
    public int Width { get; set; }
    public int Height { get; set; }
    public Vector2Int Position {  get; set; }

    public WindowedProperties(int width, int height, Vector2Int position)
    {
        Width = width;
        Height = height;
        Position = position;
    }
}

public class FullscreenManager : MonoBehaviour
{
    private TextMeshProUGUI buttonText;

    private WindowedProperties windowedProperties;

    private readonly string fullScreenText = "Completa";
    private readonly string windowedText = "Ventana";

    /// <summary>
    /// If the <see cref="Screen"/> is fullScreen, set the <see cref="buttonText"/> text as the <see cref="fullScreenText"/> one
    /// If not, use <see cref="windowedText"/>
    /// </summary>
    private void Awake()
    {
        windowedProperties = new WindowedProperties(Screen.width, Screen.height, Screen.mainWindowPosition);

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
            Screen.SetResolution(windowedProperties.Width, windowedProperties.Height, false);
            Screen.MoveMainWindowTo(Screen.mainWindowDisplayInfo, windowedProperties.Position);

            UpdateButtonText(windowedText);
        }
        else
        {
            windowedProperties.Width = Screen.width;
            windowedProperties.Height = Screen.height;
            windowedProperties.Position = Screen.mainWindowPosition;

            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

            UpdateButtonText(fullScreenText);
        }
    }

    /// <summary>
    /// Change the <see cref="buttonText"/> text to the provided <paramref name="newText"/>
    /// </summary>
    /// <param name="newText">Text to change the button with</param>
    private void UpdateButtonText(string newText)
    {
        if (buttonText == null)
        {
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        buttonText.text = newText;
    }
}


