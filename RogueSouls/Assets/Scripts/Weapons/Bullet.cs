using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolObject
{
    [SerializeField]
    protected GameObject hitEffect;
    [SerializeField]
    protected GameObject bloodHitEffect;
    [SerializeField]
    protected TrailRenderer _trailRenderer;
    [SerializeField]
    protected float bulletLife = 2f;
    [SerializeField]
    protected int bulletDamage;
    [SerializeField]
    protected RangedWeapon weapon;

    public virtual void OnEnable()
    {
        //_trailRenderer = GetComponent<TrailRenderer>();
        //CancelInvoke("OnDespawn");
        Invoke("OnDeSpawn", bulletLife);
    }

    // Start is called before the first frame update
    public virtual void OnCollisionEnter2D(Collision2D other) 
	{
        if(other.gameObject.tag == "enemy")
        {
            Enemy enemyToHit = other.gameObject.GetComponent<Enemy>();
            enemyToHit.TakeDamage(bulletDamage);
            if(hitEffect != null)
            {
                PoolObject tempEffect = PoolManager.Instance.Spawn(bloodHitEffect.name);
                tempEffect.transform.position = transform.position;
                tempEffect.GetComponent<ParticleSystem>().Play();
            }
            OnDeSpawn();
        }
        else if (other.gameObject.tag != "Player")
        {
            OnDeSpawn();
            if(hitEffect != null)
            {
                PoolObject tempEffect = PoolManager.Instance.Spawn(hitEffect.name);
                tempEffect.transform.position = transform.position;
                tempEffect.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    public void AssignWeapon(RangedWeapon weaponToAssign)
    {
        weapon = weaponToAssign;
        bulletDamage = weapon.AssignDamage();
    }
}
