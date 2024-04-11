using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    string[] _dialogue;

    [field: SerializeField]
    public string NPCName { get; private set; }

    int index = 0;

    HUD _playerHUD;

    GameObject _dialogueBox;

    private void Awake()
    {
        _playerHUD = FindObjectOfType<HUD>();
        _dialogueBox = _playerHUD.ChatBox;
    }

    public void ContinueDialogue()
    {
        if(_dialogueBox.activeSelf)
        {
            if (index < _dialogue.Length && _dialogue[index] != null)
            {
                _playerHUD.ShowSpecificMessageOnChatBox(_dialogue[index]);
                index++;
            }
            else
            {
                index = 0;
                _playerHUD.CloseChatBox();
            }
        }
    }

    public void ResetIndex()
    {
        index = 0;
    }
}
