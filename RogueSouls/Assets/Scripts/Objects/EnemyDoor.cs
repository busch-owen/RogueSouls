using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoor : Door
{
    [SerializeField]
    public List<Enemy> enemies;
    public bool killedAll = false;
    
    private void FixedUpdate()
    {

    }

    public void NotifyEnemyDied(Enemy enemy)
    {
        // Remove the dead enemy from the list
        enemies.Remove(enemy);

        // If all enemies are dead, open the door
        if (enemies.Count == 0)
        {
            OpenDoor();
        }
    }
}
