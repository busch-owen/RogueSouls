using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField]
    List<SpriteRenderer> _assetsInRoom = new();

    List<Vector2> _enemiesInRoomTransforms = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int enemyIndex = 0;

            foreach (SpriteRenderer sprite in _assetsInRoom)
            {
                sprite.enabled = true;

                if(sprite.GetComponentInParent<Enemy>())
                {
                    sprite.transform.parent.position = _enemiesInRoomTransforms[enemyIndex];
                    enemyIndex++;
                }
            }
            for (int i = 0; i < _assetsInRoom.Count; i++)
            {
                if (_assetsInRoom[i].GetComponentInParent<Enemy>())
                {
                    if (!_enemiesInRoomTransforms.Contains(_assetsInRoom[i].transform.parent.position))
                    {
                        _enemiesInRoomTransforms.Add(_assetsInRoom[i].transform.parent.position);
                    }
                    
                }
            }
        }

        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("bullet"))
        {
            if(!_assetsInRoom.Contains(other.GetComponentInChildren<SpriteRenderer>()))
            {
                _assetsInRoom.Add(other.GetComponentInChildren<SpriteRenderer>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (SpriteRenderer sprite in _assetsInRoom)
            {
                sprite.enabled = false;
            }
        }

        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("bullet"))
        {
            if (!_assetsInRoom.Contains(other.GetComponentInChildren<SpriteRenderer>()))
            {
                _assetsInRoom.Remove(other.GetComponentInChildren<SpriteRenderer>());
            }
        }
    }
}
