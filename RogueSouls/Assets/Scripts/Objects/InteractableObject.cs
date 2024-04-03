using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    bool isTriggerObject;

    [SerializeField]
    Door _targetDoor;

    [SerializeField]
    Sprite _onSprite, _offSprite;

    SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _renderer.sprite = _offSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
                _renderer.sprite = _onSprite;
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() && isTriggerObject)
        {
            if (!_targetDoor.IsOpen)
            {
                _targetDoor.OpenDoor();
                _renderer.sprite = _onSprite;
            }
            else
            {
                _targetDoor.CloseDoor();
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
            _renderer.sprite = _offSprite;
        }
    }
}
