using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class LavaProjectile : EnemyBullet
{

    [SerializeField]
    int damage;
    PlayerController playerController;
    EntityStats player;
    [SerializeField]
    ParticleSystem FireAOEEffect;
    [SerializeField]
    private float AOERange;
    Rigidbody2D rb;
    TrailRenderer TrailRenderer;
    CircleCollider2D circleCollider;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<EntityStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        TrailRenderer.enabled = false;
        rb.velocity = Vector3.zero;
        rb.freezeRotation = true;
        GameObject particleObject = PoolManager.Instance.Spawn(FireAOEEffect.name).gameObject;
        particleObject.transform.position = transform.position;
        ParticleSystem newParticle = particleObject.GetComponent<ParticleSystem>();
        newParticle?.Play();
        InvokeRepeating("LavaDamage", 0f, 1f);
        OnDeSpawn();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CancelInvoke("LavaDamage");
        OnDeSpawn();
    }
    private void LavaDamage()
    {
        playerController._xSpeed = 100;
        playerController._ySpeed = 100;
        player.TakeDamage(damage);


    }

}
