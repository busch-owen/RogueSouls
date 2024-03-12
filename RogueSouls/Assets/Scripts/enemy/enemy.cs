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
    [SerializeField]
    float _rotateSpeed;
    float _enemyWeaponRotationAngle;
    bool targetInRange;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = _speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }


    private void Update()
    {
        if (targetInRange)
        {
            enemyGun.Shoot();
            RangedAttack();
        }
    }

    private void FixedUpdate()
    {
        agent.SetDestination(target.position);
    }

    private void RangedAttack()
    {
        _enemyWeaponRotationAngle = Mathf.Atan2(target.transform.position.y - this.transform.position.y, target.transform.position.x - this.transform.position.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(_enemyWeaponRotationAngle, Vector3.forward);
        gunLocation.rotation = Quaternion.Slerp(gunLocation.rotation, rotation, _rotateSpeed * Time.deltaTime);
        Debug.Log(target.transform.position);
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            targetInRange = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            targetInRange = false;
        }
    }




}
