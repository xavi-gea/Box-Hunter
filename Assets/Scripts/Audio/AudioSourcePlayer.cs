using UnityEngine;

/// <summary>
/// Play the provided audio source
/// </summary>
public class AudioSourcePlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        if (audioSource != null) 
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
