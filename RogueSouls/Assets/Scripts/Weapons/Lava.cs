using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{

    [SerializeField]
    protected GameObject hitEffect;
    EntityStats stats;
    [SerializeField]
    int damage;
    PlayerController playerController;
    [SerializeField]
    float LavaDamageTick;

    private void OnTriggerEnter2D(Collider2D other)
    {

        
        if (other.GetComponent<EntityStats>())
        {
            playerController = other.GetComponent<PlayerController>();
            
            stats = other.GetComponent<EntityStats>();
            InvokeRepeating("LavaDamage", 0f, LavaDamageTick);
        }
         
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<EntityStats>())
        {   
            stats = other.GetComponent<EntityStats>();
            CancelInvoke("LavaDamage");
            playerController._xSpeed = 420;
            playerController._ySpeed = 300;


        }

    }

    private void LavaDamage()
    {
        playerController._xSpeed = 100;
        playerController._ySpeed = 100;
        stats.TakeDamage(damage);
        

    }

    
}
