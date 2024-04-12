using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    // The value of sound volume that is set to be unaffected by scene transitions.
    public static float GlobalSoundVolumeValue { get; private set; }

    // These panels dynamically open and close within the menu to provide menu functionality.
    [Header("Menu Panel Objects")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _creditsPanel;

    // Slider used for changing the sound.
    [Header("Other Objects")]
    [SerializeField] private Slider _soundSlider;

    private void Awake()
    {
        // Load the sound value via PlayerPrefs, starting with a default value of 1 if no record exists.
        GlobalSoundVolumeValue = PlayerPrefs.GetFloat("SoundVolume", 1f);

        // Update the slider value based on the loaded value.
        _soundSlider.value = GlobalSoundVolumeValue;
    }

    public void StartGame(string startSceneName = "Yasub_Scene")
    {
        // Continue if the specified start scene exists
        if (SceneUtility.GetBuildIndexByScenePath(startSceneName) != -1)
        {
            // Load the start scene, replace "GameScene" with the name of your game scene.
            SceneManager.LoadScene(startSceneName);
        }
        else
        {
            Debug.LogWarning("There is no scene with the entered scene name or it has not been added to the build settings.");
        }
    }

    public void SetOptionPanelActive(bool value)
    {
        // If the incoming value is true, options open; otherwise, they close.
        _optionsPanel.SetActive(value);
    }

    public void SetCreditsPanelActive(bool value)
    {
        // If the incoming value is true, credits open; otherwise, they close.
        _creditsPanel.SetActive(value);
    }

    public void SetMainMenuPanelActive(bool value)
    {
        // If the incoming value is true, the main menu opens; otherwise, it closes.
        _mainMenuPanel.SetActive(value);
    }

    public void ChangeVolumeOnValueChanged()
    {
        // Sound volume is set based on the value of the slider.
        GlobalSoundVolumeValue = _soundSlider.value / _soundSlider.maxValue;
    }

    public void QuitGame()
    {
        // Save the sound value via PlayerPrefs.
        PlayerPrefs.SetFloat("SoundVolume", GlobalSoundVolumeValue);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // Quit from the game.
#endif
    }
    private void OnApplicationQuit()
    {
        // Save the sound value via PlayerPrefs.
        PlayerPrefs.SetFloat("SoundVolume", GlobalSoundVolumeValue);
    }
}
