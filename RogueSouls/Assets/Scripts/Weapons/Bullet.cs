using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolObject
{
    [SerializeField]
    protected GameObject hitEffect;
    [SerializeField]
    protected TrailRenderer _trailRenderer;
    [SerializeField]
    protected float bulletLife = 2f;
    [SerializeField]
    protected int bulletDamage;
    [SerializeField]
    protected RangedWeapon weapon;


    // Start is called before the first frame update
    public virtual void OnCollisionEnter2D(Collision2D other) 
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

    public virtual void OnEnable()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        Invoke("OnDeSpawn", bulletLife);
        
    }

    public void AssignWeapon(RangedWeapon weaponToAssign)
    {
        weapon = weaponToAssign;
        bulletDamage = weapon.AssignDamage();
    }
}
