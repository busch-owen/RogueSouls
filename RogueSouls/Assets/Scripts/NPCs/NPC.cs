using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class NPC : MonoBehaviour
{
    [SerializeField]
    string[] _dialogue;

    [field: SerializeField]
    public string NPCName { get; private set; }

    int index = 0;

    HUD _playerHUD;

    GameObject _dialogueBox;
    
    [SerializeField]
    private GameObject _exclaimationMark;

    private void Awake()
    {
        _playerHUD = FindObjectOfType<HUD>();
        _dialogueBox = _playerHUD.ChatBox;
    }

    public void ContinueDialogue()
    {
        if (!_dialogueBox.activeSelf) return;
        
        if (_exclaimationMark)
        {
            _exclaimationMark.SetActive(false);
        }
        
        if (index < _dialogue.Length && _dialogue[index] != null)
        {
            _playerHUD.ShowSpecificMessageOnChatBox(NPCName + ": \n" + _dialogue[index] + "\n\nPress [Interact] to continue");
            index++;
        }
        else
        {
            index = 0;
            _playerHUD.CloseChatBox();
        }
    }

    public void ResetIndex()
    {
        index = 0;
    }
}
