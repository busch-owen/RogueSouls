using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class LavaProjectile : EnemyExplosion
{

    [SerializeField]
    int damage;
    PlayerController playerController;
    EntityStats player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<EntityStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

}
