using UnityEngine;

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
