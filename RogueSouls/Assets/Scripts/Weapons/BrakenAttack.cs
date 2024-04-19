using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BrakenAttack : RangedWeapon
{
    [SerializeField]
    public Bullet Lava, OtherLava;
   
    

    int rand;

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
            rand = Random.Range(0, 10);

            Debug.Log(rand.ToString());
            if (rand >=5)
            {

                bulletForce = 1;
                bulletPrefab = Lava;
            }
            else if(rand <=5)
            {
                bulletForce = 15;
                bulletPrefab = OtherLava;
            }
        }

        base.Shoot(additionalVelocity);
    }


}
