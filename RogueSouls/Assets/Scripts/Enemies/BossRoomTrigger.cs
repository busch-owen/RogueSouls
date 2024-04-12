using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
    [SerializeField]
    Enemy _bossInRoom;

    [SerializeField]
    Door _doorToOpenUponBossDeath;

    private void FixedUpdate()
    {
        if(_bossInRoom == null)
        {
            _doorToOpenUponBossDeath.OpenDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            _bossInRoom.SetTarget(collision.transform);
        }
    }
}
