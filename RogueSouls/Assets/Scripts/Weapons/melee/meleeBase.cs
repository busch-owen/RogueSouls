using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeBase : MonoBehaviour
{
    [SerializeField]
    PlayerController _playerController;

    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _range;
    [SerializeField]
    private float _knockback;
    [SerializeField]
    private float _stamina;
    [SerializeField]
    private float _cooldown;
    [SerializeField]
    private float _swingSpeed;
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    float _invulnTime;

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
        StartCoroutine(BeginLunge());
    }

    IEnumerator BeginLunge()
    {
        if (_closestEnemy != null)
        {
            while (true)
            {
                _playerController.transform.position = Vector2.Lerp(_playerController.transform.position, _closestEnemy.transform.position, _lungeSpeed * Time.fixedDeltaTime);
                _playerController.GoInvulnerable(_invulnTime);
                yield return new WaitForFixedUpdate();
            }
        }   
    }

    void DetermineClosestEnemy()
    {
        foreach(Enemy enemy in _enemiesInRange)
        {
            if(Vector2.Distance(enemy.transform.position, this.transform.position) < _leastDistance)
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
            DetermineClosestEnemy();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>())
        {
            _enemiesInRange.Remove(other.GetComponent<Enemy>());
            DetermineClosestEnemy();
        }
    }

}
