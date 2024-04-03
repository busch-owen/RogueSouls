using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeBase : MonoBehaviour
{
    [SerializeField]
    PlayerController _playerController;

    Rigidbody2D _playerRB;

    [SerializeField]
    private int _damage;
    [SerializeField]
    private float _knockback;
    bool _canLunge;
    bool _isLunging;
    [SerializeField]
    private float _cooldown;

    [SerializeField]
    float _invulnTime;

    List<Enemy> _enemiesInRange = new List<Enemy>();

    float _leastDistance;
    [SerializeField]
    Enemy _closestEnemy;

    [SerializeField]
    float _lungeSpeed;

    ScreenShakeEffect _shakeEffect;

    Vector2 _lungeDirection;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _shakeEffect = GetComponent<ScreenShakeEffect>();
        _canLunge = true;
        _playerRB = GetComponentInParent<Rigidbody2D>();
    }

    public virtual void Attack()
    {
        DetermineClosestEnemy();
        if(_closestEnemy != null && _canLunge)
        {
            StartCoroutine(BeginLunge());
        }
    }

    void AllowLunge()
    {
        _canLunge = true;
    }

    IEnumerator BeginLunge()
    {
        _playerController.ToggleDashSmear(true);
        _isLunging = true;
        while (true)
        {
            _lungeDirection = _closestEnemy.transform.position - _playerController.transform.position;
            _leastDistance = Vector2.Distance(_closestEnemy.transform.position, this.transform.position);
            //_playerController.transform.position = Vector2.Lerp(_playerController.transform.position, _closestEnemy.transform.position, _lungeSpeed * Time.fixedDeltaTime);
            _playerRB.AddForce(_lungeDirection * _lungeSpeed);
            _playerController.GoInvulnerable(_invulnTime);
            _canLunge = false;
            DealDamage();
            yield return new WaitForFixedUpdate();
        }
    }
    void DealDamage()
    {
        if(_leastDistance < 1)
        {
            _shakeEffect.ShakeScreen();
            _closestEnemy.TakeDamage(_damage);
            StopAllCoroutines();
            _playerController.ToggleDashSmear(false);
            Invoke("AllowLunge", _cooldown);
            _isLunging = false;
        }
    }

    void DetermineClosestEnemy()
    {
        foreach(Enemy enemy in _enemiesInRange)
        {
            if (Vector2.Distance(enemy.transform.position, this.transform.position) < _leastDistance)
            {
                _closestEnemy = enemy;
                _leastDistance = Vector2.Distance(enemy.transform.position, this.transform.position);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Enemy>())
        {
            _enemiesInRange.Add(other.GetComponent<Enemy>());
            if (_closestEnemy == null)
            {
                _closestEnemy = other.GetComponent<Enemy>();
                _leastDistance = Vector2.Distance(_closestEnemy.transform.position, this.transform.position);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>())
        {
            _enemiesInRange.Remove(other.GetComponent<Enemy>());

            if (_enemiesInRange.Count == 0)
            {
                _closestEnemy = null;
            }
        }
    }

    public bool IsLunging()
    {
        return _isLunging;
    }

}
