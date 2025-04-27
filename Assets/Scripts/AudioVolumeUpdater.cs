using UnityEngine;
using UnityEngine.UI;

public class AudioVolumeUpdater : MonoBehaviour
{
    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = gameObject.GetComponent<Slider>();
    }

    public void UpdateVolume()
    {
        if (volumeSlider == null)
        {
            volumeSlider = gameObject.GetComponent<Slider>();
        }

        AudioListener.volume = volumeSlider.value;
    }
}
