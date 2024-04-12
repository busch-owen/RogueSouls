using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KingSlime : Enemy
{
    [SerializeField]
    Image _barFillImage;

    [SerializeField]
    TMP_Text _barHealthText;

    protected override void Start()
    {
        base.Start();
        _barHealthText.text = Health + "/" + _maxHealth;
    }

    public override void Update()
    {
        if (target != null)
        {
            enemyGun.Shoot();
            RangedAttack();
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);

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
        _barFillImage.fillAmount = (float)Health / (float)_maxHealth;
        _barHealthText.text = Health + "/" + _maxHealth;
    }
}
