using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField]
    GameObject _textBox;

    TMP_Text _textBoxText;

    PlayerController _player;

    private void Awake()
    {
        _textBoxText = _textBox.GetComponentInChildren<TMP_Text>();
        _textBox.SetActive(false);
        _player = GetComponentInParent<PlayerController>();
    }

    #region Text Box

    public void ShowSpecificMessage(string message, float messageLength)
    {
        _textBox.SetActive(true);
        _textBoxText.text = message + "\n\nPress [ESC] to close.";
        Invoke("CloseTextBox", messageLength);
    }

    public void CloseTextBox()
    {
        if(_textBox != null && _textBox.activeSelf)
        {
            _textBoxText.text = "";
            _player.AllowInput();
            _textBox.SetActive(false);
        }
        
    }

    #endregion
}
