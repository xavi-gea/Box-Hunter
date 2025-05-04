using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource audioSource;

    public AudioClip buttonSoundClip;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonClip()
    {
        if (Instance == this)
        {
            audioSource.PlayOneShot(buttonSoundClip);
        }
        else
        {
            Instance.PlayButtonClip();
        }
    }
}
