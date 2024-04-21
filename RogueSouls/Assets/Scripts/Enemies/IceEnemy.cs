using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEnemy : Enemy
{
    [SerializeField]
    bool iceArmor;
    [SerializeField]
    int armorDamper = 50;


    public override void TakeDamage(int damage)
    {
        if (iceArmor)
        {
            damage *= (armorDamper / 100);
        }
        base.TakeDamage(damage);
        //if( _health > 50%) // If health Falls below 50 the icearmor gets removed and takes straight damage
        {
        }
    }
}
