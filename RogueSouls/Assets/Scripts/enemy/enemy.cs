using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    [SerializeField] Transform target;

    NavMeshAgent agent;
    [SerializeField]
    float health;

    

    



    private void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
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

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
