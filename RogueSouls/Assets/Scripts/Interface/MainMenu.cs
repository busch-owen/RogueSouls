using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Yasub_Scene"); // Replace "GameScene" with the name of your game scene.
    }

    public void OpenOptions()
    {
        // It is going to be edited due to what we need.
        Debug.Log("Options menu opened."); // Placeholder for now.
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // Quit from the game.
#endif
    }
}
