using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSwitch : MonoBehaviour
{
    public bool IsTriggered = false;
    [field: SerializeField]
    public Sprite _onSprite, _offSprite;
    [SerializeField]
    TargetDoor _door;

    public SpriteRenderer _renderer { get; protected set; }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Bullet>())
        {
           

            if (!IsTriggered)
            {
                IsTriggered = true;
                _door.CheckTargets(this);
                _renderer.sprite = _onSprite;
            }
            else
            {
                IsTriggered = false;
                _door.DecreaseIndex();
                _renderer.sprite = _offSprite;
            }
            

        }
    }

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void ResetSwitch()
    {
        IsTriggered = false;
        _renderer.sprite = _offSprite;
    }
}
