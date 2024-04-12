using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KingSlime : Enemy
{
    [SerializeField]
    Image _barFillImage;

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

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        Debug.Log(Health / _maxHealth);
        _barFillImage.fillAmount = (float)Health / (float)_maxHealth;
    }
}
