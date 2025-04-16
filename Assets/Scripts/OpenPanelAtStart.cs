using UnityEngine;

public class OpenPanelAtStart : MonoBehaviour
{
    public GameObject panelToOpen;

    void Start()
    {
        ScreenManager.Instance.OpenPanel(panelToOpen);
    }
}
