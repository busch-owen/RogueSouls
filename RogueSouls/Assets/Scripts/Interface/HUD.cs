using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField]
    GameObject _textBox;

    [field: SerializeField]
    public GameObject ChatBox { get; private set; }

    TMP_Text _textBoxText, _chatBoxText;

    PlayerController _player;

    [SerializeField]
    TMP_Text _promptText;
    [SerializeField]
    GameObject _promptStartPos, _promptEndPos;

    [SerializeField]
    float _promptLerpSpeed;

    WaitForFixedUpdate _waitForFixedUpdate;

    private void Awake()
    {
        _textBoxText = _textBox.GetComponentInChildren<TMP_Text>();
        _textBox.SetActive(false);
        _chatBoxText = ChatBox.GetComponentInChildren<TMP_Text>();
        ChatBox.SetActive(false);
        _player = GetComponentInParent<PlayerController>();
        _promptText.transform.position = _promptStartPos.transform.position;
    }

    #region Text Box

    private void FixedUpdate()
    {
        if (_player.InRangeOfNPC)
        {
            OpenPromptText("Press [F] or [South Face Button] to speak to " + _player.CurrentNPC.NPCName);
        }  
        else if (_player.InRangeOfChest && !_player.CurrentChest.Opened)
        {
            OpenPromptText("Press [F] or [West Face Button] to open chest.");
        }
        else if (_player.InRangeOfDoor && _player.CurrentDoor.IsLocked)
        {
            OpenPromptText("Press [F] or [West Face Button] to unlock door.");
        }
        else
        {
            ClosePromptText();
        }
    }

    public void ShowSpecificMessageOnTextBox(string message, float messageLength)
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
    public void OpenChatBox()
    {
        if(ChatBox != null)
        {
            ChatBox.SetActive(true);
        }
    }

    public void ShowSpecificMessageOnChatBox(string message)
    {
        _chatBoxText.text = message;
    }

    public void CloseChatBox()
    {
        if (ChatBox != null && ChatBox.activeSelf)
        {
            _chatBoxText.text = "";
            _player.AllowInput();
            ChatBox.SetActive(false);
        }
    }

    public void OpenPromptText(string whatToDisplay)
    {
            _promptText.transform.position = Vector2.Lerp(_promptText.transform.position, _promptEndPos.transform.position, _promptLerpSpeed * Time.fixedDeltaTime);
            _promptText.text = whatToDisplay;
    }

    public void ClosePromptText()
    {
        _promptText.transform.position = Vector2.Lerp(_promptText.transform.position, _promptStartPos.transform.position, _promptLerpSpeed * Time.fixedDeltaTime);
    }

    #endregion
}
