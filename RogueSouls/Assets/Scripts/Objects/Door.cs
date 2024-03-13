using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [field: SerializeField]
    public bool IsLocked { get; private set; }

    public bool IsOpen { get; private set; }

    GameObject _doorObject;

    [SerializeField]
    Sprite _unlockedSprite, _lockedSprite;

    SpriteRenderer _doorRenderer;

    private void Start()
    {
        _doorRenderer = GetComponentInChildren<SpriteRenderer>();
        _doorObject = _doorRenderer.gameObject;

        if(IsLocked)
        {
            _doorRenderer.sprite = _lockedSprite;
        }
        else
        {
            _doorRenderer.sprite = _unlockedSprite;
        }
        
    }

    public void OpenDoor()
    {
        if(!IsLocked)
        {
            _doorObject.SetActive(false);
            IsOpen = true;
        }
    }

    public void CloseDoor()
    {
        _doorObject.SetActive(true);
        IsOpen = false;
    }

    public void UnlockDoor()
    {
        if(IsLocked)
        {
            _doorRenderer.sprite = _unlockedSprite;
            IsLocked = false;
        }
    }
}
