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

    MinionSlime[] _enemiesSpawned;

    protected virtual void OnEnable()
    {
        _barFillImage.fillAmount = (float)Health / (float)_maxHealth;
        _barHealthText.text = Health + "/" + _maxHealth;
    }

    protected override void Update()
    {
        if (target)
        {
            enemyGun.Shoot();
            RangedAttack();
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);

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
        if (target && _agent.isActiveAndEnabled)
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

    public void DespawnAllSlimesSpawned()
    {
        _enemiesSpawned = FindObjectsOfType<MinionSlime>();
        foreach(MinionSlime enemySpawned in _enemiesSpawned)
        {
            enemySpawned.OnDeSpawn();
        }
    }
}
