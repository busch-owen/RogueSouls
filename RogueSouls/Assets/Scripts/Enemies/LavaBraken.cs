using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBraken : Enemy
{
    public override void Update()
    {
        if (target != null)
        {
            enemyGun.Shoot();
            RangedAttack();
        }

        bool flipSprite = agent.velocity.x < 0;

        if (flipSprite)
        {
            enemySprite.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            enemySprite.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public override void FixedUpdate()
    {
        if (target != null && agent.isActiveAndEnabled)
        {
            agent.SetDestination(target.position);
        }
    }
}
