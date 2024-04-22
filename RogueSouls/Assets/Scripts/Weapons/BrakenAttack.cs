using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BrakenAttack : RangedWeapon
{
    [SerializeField]
    public Bullet Fire, Lava;
    [SerializeField]
    Enemy Enemy;
    [SerializeField]
    float seconds;
    [SerializeField]
    float Max;
    [SerializeField]
    float Min;
   
    

    int rand;
    float randInt;

    private void Start()
    {
        
        
    }

    private void Update()
    {

    }

    public override void Shoot(Vector2 additionalVelocity = default)
    {
        
        
        if(Time.time < timeToNextFire )
        {
            randInt = Random.Range( Min, 1000 );
            rand = Random.Range(0, 100);


            
            if(randInt >= 999)
            {
                
                EnemySpawn();
            }
            if (rand >=25)
            {
                bulletForce = 10;
                fireRate = 1;
                bulletPrefab = Fire;
            }
            else if(rand <=25)
            {
                fireRate = 1;
                bulletForce = 20;
                bulletPrefab = Lava;
            }

        }

        base.Shoot(additionalVelocity);
    }

    public void EnemySpawn()
    {
        Instantiate(Enemy, firePoint.position, Quaternion.identity);
    }


}
