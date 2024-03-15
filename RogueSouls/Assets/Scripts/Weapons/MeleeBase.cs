using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeBase : MonoBehaviour
{
    [SerializeField]
    PlayerController _playerController;

    [SerializeField]
    private int _damage;
    [SerializeField]
    private float _knockback;
    [SerializeField]
    private float _cooldown;

    [SerializeField]
    float _invulnTime;

    [SerializeField]
    List<Enemy> _enemiesInRange = new List<Enemy>();

    float _leastDistance;
    [SerializeField]
    Enemy _closestEnemy;

    [SerializeField]
    float _lungeSpeed;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
    }

    public virtual void Attack()
    {
        DetermineClosestEnemy();
        StartCoroutine(BeginLunge());
    }

    IEnumerator BeginLunge()
    {

        while (true)
        {
            if (_closestEnemy != null)
            {
                _playerController.transform.position = Vector2.Lerp(_playerController.transform.position, _closestEnemy.transform.position, _lungeSpeed * Time.fixedDeltaTime);
                _playerController.GoInvulnerable(_invulnTime);
                
                //DealDamage();
            }
            yield return new WaitForFixedUpdate();
        }
    }
    void DealDamage()
    {
        _closestEnemy.TakeDamage(_damage);
        StopCoroutine(BeginLunge());
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

}
