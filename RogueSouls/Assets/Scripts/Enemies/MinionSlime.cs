using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class MinionSlime : EnemyBullet
{
    [SerializeField] Transform _target;
    public NavMeshAgent _agent { get; protected set; }

    Rigidbody2D _rb;

    [SerializeField]
    private RangedWeapon enemyGun;
    [SerializeField]
    private Transform gunLocation;
    [SerializeField]
    float _rotateSpeed;
    float _enemyWeaponRotationAngle;

    [SerializeField]
    public float _speed { get; protected set; }

    int _health;
    [SerializeField]
    int _maxHealth;

    public GameObject _spriteObject { get; protected set; }

    public override void OnEnable()
    {
        _health = _maxHealth;
        _target = FindObjectOfType<PlayerController>().transform;

        if(_rb == null)
            _rb = gameObject.AddComponent<Rigidbody2D>();

        _rb.gravityScale = 0;
        _rb.simulated = true;
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.speed = _speed;
        _agent.enabled = false;
        _spriteObject = GetComponentInChildren<SpriteRenderer>().gameObject;
    }

    public virtual void Update()
    {
        if(_target != null)
        {
            enemyGun.Shoot();
            RangedAttack();
        }
        

        _spriteObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        bool flipSprite = _agent.velocity.x < 0;

        if (flipSprite)
        {
            _spriteObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            _spriteObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public override void FixedUpdate()
    {
        if(_agent != null && _agent.isActiveAndEnabled)
        {
            _agent.SetDestination(_target.position);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        IncrementHealth(-damage);

        if (_health <= 0)
        {
            OnDeSpawn();
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerStats>())
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(bulletDamage);
        }

        Destroy(_rb);
        _agent.enabled = true;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public virtual void IncrementHealth(int incrementAmount)
    {
        if (_health <= 0)
        {
            return;
        }
        _health += incrementAmount;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
    }

    private void RangedAttack()
    {
        _enemyWeaponRotationAngle = Mathf.Atan2(_target.transform.position.y - this.transform.position.y, _target.transform.position.x - this.transform.position.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(_enemyWeaponRotationAngle, Vector3.forward);
        gunLocation.rotation = Quaternion.Slerp(gunLocation.rotation, rotation, _rotateSpeed * Time.deltaTime);
    }
}
