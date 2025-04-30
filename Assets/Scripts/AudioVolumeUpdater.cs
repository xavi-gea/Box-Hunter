using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeUpdater : MonoBehaviour
{
    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = gameObject.GetComponent<Slider>();
    }

    /// <summary>
    /// Sets the global audio volume to be the same as <see cref="volumeSlider"/> value
    /// </summary>
    public void UpdateVolume()
    {
        if (volumeSlider == null)
        {
            volumeSlider = gameObject.GetComponent<Slider>();
        }

        AudioListener.volume = volumeSlider.value;
    }
}
