using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{

 
    EntityStats stats;
    [SerializeField]
    int damage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        
        if (other.GetComponent<EntityStats>())
        {
            stats = other.GetComponent<EntityStats>();
            InvokeRepeating("LavaDamage", 0f, 1f);
            Debug.Log(damage);


        }
         
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<EntityStats>())
        {
            stats = other.GetComponent<EntityStats>();
            CancelInvoke("LavaDamage");
            

        }

    }

    private void LavaDamage()
    {
        stats.TakeDamage(damage);
    }

    
}
