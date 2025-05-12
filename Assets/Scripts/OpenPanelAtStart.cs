using UnityEngine;

/// <summary>
/// When this loads, enable the provided panel gameobject
/// </summary>
public class OpenPanelAtStart : MonoBehaviour
{
    public GameObject panelToOpen;

    void Start()
    {
        ScreenManager.Instance.OpenPanel(panelToOpen);
    }
}
