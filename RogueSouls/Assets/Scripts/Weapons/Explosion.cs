using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Bullet
{
    [SerializeField]
    float timeActive = 4f;
    [SerializeField]
    private CircleCollider2D circleCollider;
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    ParticleSystem FireAOEEffect;
    bool isRocket = false;
    bool hasCollided = false;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if (timeActive <= 0)
        {
            FireAOEEffect?.Stop();
        }

        if(hasCollided)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }
        
    }

    public override void OnEnable()
    {
        circleCollider.enabled = true;
        hasCollided = false;
        Invoke("OnDeSpawn", bulletLife);
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            return;
            
        }
        circleCollider.radius = (1.0f);
        FireAOEEffect?.Play();
        isRocket = true;
        hasCollided = true;


        //create particle system
        //create circle collider the same size as particle system
        // set velocity to 0 
    }
    private void OnTriggerStay2D(Collider2D other)
    {

            if (other.gameObject.tag == "enemy" && isRocket == true)
            {
                Enemy enemyToHit = other.gameObject.GetComponent<Enemy>();
                enemyToHit.TakeDamage(bulletDamage);
            }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

}
