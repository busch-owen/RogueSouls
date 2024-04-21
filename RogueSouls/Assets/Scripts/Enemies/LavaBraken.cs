using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LavaBraken : Enemy
{
    [SerializeField]
    Image _barFillImage;
    
    [SerializeField]
    private TMP_Text _barHealthText;

    protected override void Update()
    {
        if (target != null)
        {
            enemyGun.Shoot();
            RangedAttack();
            




        }

        bool flipSprite = _agent.velocity.x < 0;

        if (flipSprite)
        {
            enemySprite.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            enemySprite.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    protected override void FixedUpdate()
    {


        if (target != null && _agent.isActiveAndEnabled)
        {
            _agent.SetDestination(target.position);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        _barFillImage.fillAmount = (float)Health / (float)_maxHealth;
        _barHealthText.text = Health + "/" + _maxHealth;
    }
}
