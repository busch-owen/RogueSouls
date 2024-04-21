using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LavaBall : Enemy
{

    [SerializeField]
    PoolObject LavaEffect;

    public void OnCollisionEnter2D(Collision2D coll)
    {
        PoolObject particleObject = PoolManager.Instance.Spawn(LavaEffect.name);
        particleObject.transform.position = transform.position;
        particleObject.GetComponent<ParticleSystem>().Play();
        Destroy(this.gameObject);
       
    }

}
