using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyBullet : Bullet
{
    // Start is called before the first frame update
    public override void OnCollisionEnter2D(Collision2D other)
    {   
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerStats enemyToHit = other.gameObject.GetComponent<PlayerStats>();
            enemyToHit.TakeDamage(bulletDamage);

            if (bloodHitEffect)
            {
                PoolObject tempEffect = PoolManager.Instance.Spawn(bloodHitEffect.name);
                tempEffect.transform.position = transform.position;
                tempEffect.GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            if (hitEffect)
            {
                PoolObject tempEffect = PoolManager.Instance.Spawn(hitEffect.name);
                tempEffect.transform.position = transform.position;
                tempEffect.GetComponent<ParticleSystem>().Play();
            }
        }
        OnDeSpawn();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void OnEnable()
    {        
        base.OnEnable();
        _trailRenderer = GetComponent<TrailRenderer>();
        weapon = FindObjectOfType<RangedWeapon>();
        bulletDamage = weapon.AssignDamage();   
    }
}
