using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSource; // AudioSource Reference
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f; // Control the auido
    [SerializeField] private bool isMuted = false; // Mute the auido

    private void OnEnable()
    {
        audioSource = FindObjectsOfType<AudioSource>();
    }

    public void ToggleAudio()
    {
        isMuted = !isMuted;
        if (audioSource != null)
        {
            foreach (AudioSource source in audioSource)
            {
                source.volume = isMuted ? 0f : volume;
            }
        }
    }
    // Adjust the audiocontrol
    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        if(audioSource != null)
        {
            foreach(AudioSource source in audioSource)
            {
                source.volume = isMuted ? 0f : volume;
            }
        }
    }
}