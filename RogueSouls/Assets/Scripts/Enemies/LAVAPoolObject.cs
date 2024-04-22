using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LAVAPoolObject : MinionSlime
{
    [SerializeField]
    PoolObject LavaEffect;
    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        
    }
    public override void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.CompareTag("Player"))
            {
                PoolObject particleObject = PoolManager.Instance.Spawn(LavaEffect.name);
                particleObject.transform.position = transform.position;
                particleObject.GetComponent<ParticleSystem>().Play();
                Destroy(this.gameObject);
            }
            else if (coll.gameObject.CompareTag("bullet"))
            {

            }



        }

    public override void TakeDamage(int damage)
    {
        IncrementHealth(-damage);

        if (_health <= 0)
        {
            PoolObject particleObject = PoolManager.Instance.Spawn(LavaEffect.name);
            particleObject.transform.position = transform.position;
            particleObject.GetComponent<ParticleSystem>().Play();
            Destroy(this.gameObject);
        }
    }
}

