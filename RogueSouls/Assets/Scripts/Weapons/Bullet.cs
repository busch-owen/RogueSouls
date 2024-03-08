using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolObject
{
    [SerializeField]
	GameObject hitEffect;
    [SerializeField]
    float bulletLife = 2f;
    float bulletDamage;
    RangedWeapon weapon;


    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other) 
	{

        if(other.gameObject.tag == "enemy")
        {
            enemy enemyToHit = other.gameObject.GetComponent<enemy>();
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
        Invoke("OnDeSpawn", bulletLife);
        weapon = FindObjectOfType<RangedWeapon>();
        bulletDamage = weapon.AssignDamage();
    }
}
