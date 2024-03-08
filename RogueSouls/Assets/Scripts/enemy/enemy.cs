using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : EntityStats
{
    [SerializeField] Transform target;
    NavMeshAgent agent;
    [SerializeField]
    bool isRanged;

    [SerializeField]
    private RangedWeapon enemyGun;
    [SerializeField]
    private Transform gunLocation;

    float _enemyWeaponRotationAngle;


    private void Start()
    {
     
        agent = GetComponent<NavMeshAgent>();
        agent.speed = _speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
    }


    private void Update()
    {
        agent.SetDestination(target.position);
    }

    private void FixedUpdate()
    {

    }

    IEnumerator Attack()
    {
        if (isRanged)
        {
            _enemyWeaponRotationAngle = Mathf.Atan2(target.transform.position.y, target.transform.position.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(_enemyWeaponRotationAngle, Vector3.forward);
            enemyGun.transform.rotation = rotation;
            Debug.Log("eA");
            enemyGun.Shoot();
        }
        else
        {
            yield return new WaitForSeconds(_timeToAttack);
            IncrementHealth(_damage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopCoroutine(Attack());
        }
    }




}
