using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LavaBall : MinionSlime
{
    [SerializeField]
    float detectionRadius;
    [SerializeField] protected Transform target;
    bool targetInRange;
    [SerializeField] protected float BallSpeed;

    public override void OnCollisionEnter2D(Collision2D coll)
    {
        OnDeSpawn();
    }


    public override void OnEnable()
    {
        
        base.OnEnable();
        _agent.enabled = true;
        _agent.speed = BallSpeed;
    }

    private void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        _agent.speed = BallSpeed;
    }

    

    public override void FixedUpdate()
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

        if (target != null && targetInRange && _agent.isActiveAndEnabled)
        {
            _agent.SetDestination(target.position);
        }

        _spriteObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public override void Update()
    {
        
    }
}
