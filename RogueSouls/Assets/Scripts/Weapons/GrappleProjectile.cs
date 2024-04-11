using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleProjectile : Bullet
{
    Rigidbody2D _rigidbody;

    [SerializeField]
    float _grappleSpeed;
    [SerializeField]
    float _grappleInvulnTime;

    bool _couldDespawn;

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (Mathf.Approximately(transform.position.x, _controller.transform.position.x) && Mathf.Approximately(transform.position.y, _controller.transform.position.y))
        {
            OnDeSpawn();
        }

        if(!_controller.IsGrappling() && _couldDespawn)
        {
            OnDeSpawn();
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        _couldDespawn = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.simulated = true;
    }

    public override void OnCollisionEnter2D(Collision2D col)
    {
        StartCoroutine(_controller.MoveToSpecificLocation(transform.position, _grappleSpeed, _grappleInvulnTime));
        _controller.GoGrappling(_grappleInvulnTime);
        _rigidbody.simulated = false;
        _couldDespawn = true;
    }
}
