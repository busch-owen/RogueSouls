using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [field: SerializeField]
    public bool isTriggerObject { get; protected set; }

    [field: SerializeField]
    public bool IsTriggered { get; protected set; }

    [field: SerializeField]
    public Door _targetDoor { get; protected set; }

    [field: SerializeField]
    public Sprite _onSprite, _offSprite;

   public SpriteRenderer _renderer { get; protected set; }

    private void Start()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _renderer.sprite = _offSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)//pressure plate
    {
        if(collision.gameObject.GetComponent<Enemy>() || collision.gameObject.GetComponent<EnemyBullet>())
        {
            return;
        }
        if (!isTriggerObject && !collision.gameObject.GetComponent<Bullet>())
        {
            if (!_targetDoor.IsLocked)
            {
                _targetDoor.OpenDoor();
                IsTriggered = true;
                _renderer.sprite = _onSprite;
            }

        }
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)//switch
    {
        if (collision.gameObject.GetComponent<Bullet>() && isTriggerObject)
        {
            if (!_targetDoor.IsOpen)
            {
                _targetDoor.OpenDoor();
                IsTriggered = true;
                _renderer.sprite = _onSprite;
            }
            else
            {
                _targetDoor.CloseDoor();
                IsTriggered = false;
                _renderer.sprite = _offSprite;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() || collision.gameObject.GetComponent<EnemyBullet>())
        {
            return;
        }
        if(!isTriggerObject && !collision.gameObject.GetComponent<Bullet>())
        {
            _targetDoor.CloseDoor();
            IsTriggered = false;
            _renderer.sprite = _offSprite;
        }
    }
}
