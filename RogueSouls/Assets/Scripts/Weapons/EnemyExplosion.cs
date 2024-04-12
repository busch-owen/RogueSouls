using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyExplosion : Bullet
{
    [SerializeField]
    ParticleSystem FireAOEEffect;
    [SerializeField]
    private float AOERange;
    Rigidbody2D rb;
    TrailRenderer TrailRenderer;
    CircleCollider2D circleCollider;
    public Color rayColor = Color.red; // The color of the ray

    public Vector2 center2D; // The center of the circle
    public float radius = 1.0f; // The radius of the circle
    public Color color = Color.red; // The color of the circle
    public int resolution = 36; // The number of lines to draw the circle, increase for a smoother circle

    void Update()
    {

    }

    // Start is called before the first frame update
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        circleCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>(); 
        TrailRenderer = GetComponent<TrailRenderer>();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        
        // Convert the 2D center to 3D
        Vector3 center = new Vector3(center2D.x, center2D.y, 0);
        center = this.transform.position;

        // Draw the circle
        float angleStep = 360.0f / resolution;
        for (int i = 0; i <= resolution; i++)
        {
            float angle = i * angleStep;
            Vector3 pos1 = center + Quaternion.Euler(0, 0, angle - angleStep) * new Vector3(radius, 0, 0);
            Vector3 pos2 = center + Quaternion.Euler(0, 0, angle) * new Vector3(radius, 0, 0);
            Debug.DrawLine(pos1, pos2, color);
        }

        // Get all colliders that overlap the circle
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, AOERange);

        // Draw a line to each overlapping object
        foreach (Collider2D collider in colliders)
        {
            Debug.DrawLine(center, collider.transform.position, color);
        }


        TrailRenderer.enabled = false;
        rb.velocity = Vector3.zero;
        rb.freezeRotation = true;
        GameObject particleObject = PoolManager.Instance.Spawn(FireAOEEffect.name).gameObject;
        particleObject.transform.position = transform.position;
        ParticleSystem newParticle = particleObject.GetComponent<ParticleSystem>();
        newParticle?.Play();

        if (AOERange > 0)
        {
            var hitColliders = Physics2D.OverlapCircleAll(transform.position, AOERange);
            foreach (var hit in hitColliders)
            {
                
                var player = hit.GetComponent<PlayerStats>();
                if (player)
                {                    var closestPoint = hit.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(transform.position, closestPoint);

                    player.TakeDamage(bulletDamage);
                }
            }
        }
        else
        {
            var player = collision.collider.GetComponent<PlayerStats>();
            if (player)
            {
                player.TakeDamage(bulletDamage);
                if (hitEffect != null)
                {
                    PoolObject tempEffect = PoolManager.Instance.Spawn(hitEffect.name);
                    tempEffect.transform.position = transform.position;
                    tempEffect.GetComponent<ParticleSystem>().Play();
                }
            }
        }
        OnDeSpawn();



    }

}