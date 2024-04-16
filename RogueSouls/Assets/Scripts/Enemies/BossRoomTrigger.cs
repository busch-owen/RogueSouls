using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField]
    Enemy _bossInRoom;

    [SerializeField]
    Door _doorToOpenUponBossDeath;

    [SerializeField]
    Door _entranceDoor;

    private void Start()
    {
        _bossInRoom.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(_bossInRoom == null)
        {
            _doorToOpenUponBossDeath?.OpenDoor();
            _entranceDoor?.OpenDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_bossInRoom != null)
        {
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                _bossInRoom.gameObject.SetActive(true);
                _bossInRoom.SetTarget(collision.transform);
                _entranceDoor?.CloseDoor();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(_bossInRoom != null)
        {
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                _entranceDoor?.OpenDoor();
                if(_bossInRoom.GetComponent<KingSlime>())
                {
                    _bossInRoom.GetComponent<KingSlime>().DespawnAllSlimesSpawned();
                }
                    
                _bossInRoom.IncrementHealth(999999);
                _bossInRoom.SetTarget(null);
                _bossInRoom.gameObject.SetActive(false);
            }
        }
    }
}
