using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolObject
{
    [SerializeField]
	GameObject hitEffect;
    [SerializeField]
    float bulletLife = 2f;


    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other) 
	{
        this.OnDeSpawn();

    }

    void OnEnable()
    {
        Invoke("OnDeSpawn", bulletLife);
    }
}
