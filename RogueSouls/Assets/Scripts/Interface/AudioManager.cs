using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private bool isMuted; // Mute the audio
    

    private void OnEnable()
    {
        //When the scene is started, the sound volume is set to the level adjusted by the user.
        SetVolume(MainMenuHandler.GlobalSoundVolumeValue);
    }

    public void ToggleAudio()
    {
        isMuted = !isMuted;
        var newVolume = isMuted ? 0f : MainMenuHandler.GlobalSoundVolumeValue;
        PlayerPrefs.SetFloat("SoundVolume", newVolume);
    }
    // Adjust the audio control
    public void SetVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("SoundVolume", newVolume);
    }
}