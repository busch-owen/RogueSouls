using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform target;

    NavMeshAgent agent;
    EntityStats stats;
    [SerializeField]
    bool isRanged;
    private void Start()
    {
        stats = GetComponent<EntityStats>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = stats._speed;
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

    public void TakeDamage(int damage)
    {
        stats.IncrementHealth(damage);
        if (stats._health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator Attack(EntityStats otherStats)
    {
        if (isRanged)
        {

        }
        else
        {
            yield return new WaitForSeconds(stats._timeToAttack);
            otherStats.IncrementHealth(stats._damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Attack(other.GetComponent<EntityStats>()));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopCoroutine(Attack(other.GetComponent<EntityStats>()));
        }
    }




}
