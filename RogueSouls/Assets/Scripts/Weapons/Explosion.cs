using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : Bullet
{
    [SerializeField]
    private CircleCollider2D circleCollider;
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    FireEffect FireAOEEffect;
    SpriteRenderer spriteRenderer;
    bool isRocket = false;
    bool hasCollided = false;

    [SerializeField]
    float _explosionRadius, _startRadius;

    int itemsCollided = 0;

    // Start is called before the first frame update
    private void FixedUpdate()
    {
        if(hasCollided)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }
    }

    public override void OnEnable()
    {
        circleCollider.enabled = true;
        itemsCollided = 0;
        circleCollider.radius = _startRadius;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.enabled = true;
        _trailRenderer.enabled = true;
        hasCollided = false;
        Invoke("OnDeSpawn", bulletLife);
    }

    public override void OnCollisionEnter2D(Collision2D other)
    { 
        if (other.gameObject.tag == "Player")
        {
            return;
            
        }
        circleCollider.radius = _explosionRadius;
        if(itemsCollided == 0)
        {
            FireEffect tempEffect = (FireEffect)PoolManager.Instance.Spawn(FireAOEEffect.name);
            tempEffect.transform.position = this.transform.position;
        }
        spriteRenderer.enabled = false;
        _trailRenderer.enabled = false;
        isRocket = true;
        hasCollided = true;
        itemsCollided++;

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
}
