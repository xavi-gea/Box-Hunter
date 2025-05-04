using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip buttonSoundClip;
    
    private AudioSource audioSource;

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
