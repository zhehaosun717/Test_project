using UnityEngine;

/// <summary>
/// A simple component to play a looping background music track.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class CS_BGMPlayer : MonoBehaviour
{
    [Tooltip("The audio clip to be played as background music.")]
    public AudioClip backgroundMusic;
    
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.playOnAwake = true; // Ensure it plays on start, though we also call Play() for certainty.
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Background Music (BGM) clip is not assigned in the Inspector.", this);
        }
    }
}