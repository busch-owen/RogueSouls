using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : EntityStats
{
    #region Global Variables

    [SerializeField] protected Transform target;
    protected NavMeshAgent _agent;
    [SerializeField]
    bool isRanged;

    [SerializeField]
    protected RangedWeapon enemyGun;
    [SerializeField]
    private Transform gunLocation;
    [SerializeField]
    float _rotateSpeed;
    float _enemyWeaponRotationAngle;
    bool targetInRange;
    [SerializeField]
    EnemyDoor enemyDoor;

    private WeaponOffsetHandle _offsetHandle;

    protected GameObject enemySprite;

    [SerializeField]
    ParticleSystem _deathEffect;

    [SerializeField]
    float detectionRadius;

    Animator _animator;

    #endregion




    #region Start   
    [SerializeField]
    bool iceArmor;
    [SerializeField]
    int armorDamper = 50;


    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (iceArmor)
        {
            damage *= (armorDamper/100);
        }
        
        
        if (enemyDoor != null && Health <= 0)
        {
            enemyDoor.NotifyEnemyDied(this);
        }

        if (_deathEffect && Health <= 0)
        {
            Instantiate(_deathEffect, transform.position, Quaternion.identity);
        }
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = Speed;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        enemySprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        _offsetHandle = GetComponentInChildren<WeaponOffsetHandle>();

        _animator = GetComponentInChildren<Animator>();
        
        target = FindObjectOfType<PlayerController>().transform;
       
    }

#endregion
    #region Update
    protected virtual void Update()
    {
        if (target != null && targetInRange)
        {
            enemyGun.Shoot();
            RangedAttack();
        }

        bool flipSprite = _agent.velocity.x < 0;

        if(flipSprite)
        {
            enemySprite.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            enemySprite.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    protected virtual void FixedUpdate()
    {
        bool moving = _agent.velocity.x != 0 || _agent.velocity.y != 0;

        if (_animator)
        {
            _animator.SetBool("Moving", moving);
        }

        float distance = Vector3.Distance(target.position, this.transform.position);

        if (distance <= detectionRadius)
        {
            targetInRange = true;
        }
        else
        {
            targetInRange = false;
        }

        if (target != null && targetInRange && _agent.isActiveAndEnabled)
        {
            _agent.SetDestination(target.position);
        }
    }

    public virtual void RangedAttack()
    {
        _enemyWeaponRotationAngle = Mathf.Atan2(target.transform.position.y - this.transform.position.y, target.transform.position.x - this.transform.position.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(_enemyWeaponRotationAngle, Vector3.forward);
        if (_offsetHandle) _offsetHandle.OffsetWeaponPos(_enemyWeaponRotationAngle);
        gunLocation.rotation = Quaternion.Slerp(gunLocation.rotation, rotation, _rotateSpeed * Time.deltaTime);
    }
#endregion
    #region Triggers
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = other.transform;
            targetInRange = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = null;
            targetInRange = false;
        }
    }*/
    #endregion

    public void StunEnemy()
    {
        _agent.enabled = false;
        Invoke("BreakStun", 4f);
    }

    public void BreakStun()
    {
        _agent.enabled = true;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
