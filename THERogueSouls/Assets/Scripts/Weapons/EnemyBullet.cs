using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyBullet : Bullet
{
    // Start is called before the first frame update
    public override void  OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.GetComponent<PlayerStats>())
        {
            PlayerStats enemyToHit = other.gameObject.GetComponent<PlayerStats>();
            enemyToHit.TakeDamage(bulletDamage);
            this.OnDeSpawn();
        }
        else
        {
            this.OnDeSpawn();
        }

    }

    public override void OnEnable()
    {
        
        _trailRenderer = GetComponent<TrailRenderer>();
        Invoke("OnDeSpawn", bulletLife);
        weapon = FindObjectOfType<RangedWeapon>();
        bulletDamage = weapon.AssignDamage();
        
    }
}
