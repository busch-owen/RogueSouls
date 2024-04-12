using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : EntityStats
{
    #region Global Variables
    [SerializeField] protected Transform target;
    protected NavMeshAgent agent;
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

    protected GameObject enemySprite;
#endregion
    [SerializeField]
    float detectionRadius;
    

    #region Start   
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        enemySprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        
        target = FindObjectOfType<PlayerController>().transform;
       
    }
#endregion
    #region Update
    public virtual void Update()
    {
        if (target != null && targetInRange)
        {
            enemyGun.Shoot();
            RangedAttack();
        }

        bool flipSprite = agent.velocity.x < 0;

        if(flipSprite)
        {
            enemySprite.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            enemySprite.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public virtual void FixedUpdate()
    {

        
        float distance = Vector3.Distance(target.position, this.transform.position);

        if (distance <= detectionRadius)
        {
            targetInRange = true;
        }
        else
        {
            targetInRange = false;
        }

        if (target != null && targetInRange && agent.isActiveAndEnabled)
        {
            agent.SetDestination(target.position);
        }
    }

    public virtual void RangedAttack()
    {
        _enemyWeaponRotationAngle = Mathf.Atan2(target.transform.position.y - this.transform.position.y, target.transform.position.x - this.transform.position.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(_enemyWeaponRotationAngle, Vector3.forward);
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
        agent.enabled = false;
    }

    public void BreakStun()
    {
        agent.enabled = true;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
