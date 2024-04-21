using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSource; // AudioSource Reference
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f; // Control the audio
    [SerializeField] private bool isMuted; // Mute the audio

    private void OnEnable()
    {
        audioSource = FindObjectsOfType<AudioSource>();

        //When the scene is started, the sound volume is set to the level adjusted by the user.
        SetVolume(MainMenuHandler.GlobalSoundVolumeValue);
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
    // Adjust the audio control
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