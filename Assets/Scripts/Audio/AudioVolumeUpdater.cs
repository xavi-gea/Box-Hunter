using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage main audio volume
/// </summary>
public class AudioVolumeUpdater : MonoBehaviour
{
    private Slider volumeSlider;

    private void Awake()
    {
        GetVolumeSlider();
    }

    /// <summary>
    /// Gets and assigns the volume slider gameObject and default value
    /// </summary>
    private void GetVolumeSlider()
    {
        volumeSlider = gameObject.GetComponent<Slider>();
        volumeSlider.value = AudioListener.volume;
    }

    /// <summary>
    /// Sets the global audio volume to be the same as <see cref="volumeSlider"/> value
    /// </summary>
    public void UpdateVolume()
    {
        if (volumeSlider == null)
        {
            GetVolumeSlider();
        }
        else
        {
            AudioListener.volume = volumeSlider.value;
        }

        SaveManager.Instance.SavePlayerPrefs();
    }
}
