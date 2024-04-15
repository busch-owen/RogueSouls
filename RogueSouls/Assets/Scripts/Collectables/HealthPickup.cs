using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : PoolObject
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerStats>())
        {
            PlayerStats targetPlayer = other.GetComponent<PlayerStats>();
            targetPlayer.IncrementHealth(4);
            OnDeSpawn();
        }
    }
}
