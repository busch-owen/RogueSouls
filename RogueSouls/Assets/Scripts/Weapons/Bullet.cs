using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolObject
{
    [SerializeField]
	GameObject hitEffect;
    [SerializeField]
    TrailRenderer _trailRenderer;
    [SerializeField]
    float bulletLife = 2f;
    int bulletDamage;
    RangedWeapon weapon;


    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other) 
	{

        if(other.gameObject.tag == "enemy")
        {
            Enemy enemyToHit = other.gameObject.GetComponent<Enemy>();
            enemyToHit.TakeDamage(bulletDamage);
            this.OnDeSpawn();
        }
        else
        {
            this.OnDeSpawn();
        }

    }

    void OnEnable()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        Invoke("OnDeSpawn", bulletLife);
        weapon = FindObjectOfType<RangedWeapon>();
        bulletDamage = weapon.AssignDamage();
    }
}
