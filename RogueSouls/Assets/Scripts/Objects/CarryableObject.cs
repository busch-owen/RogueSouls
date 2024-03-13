using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryableObject : MonoBehaviour
{
    [SerializeField]
    bool _destructibleObject;

    GameObject _carryableObject;

    [SerializeField]
    ParticleSystem _destructionEffect;

    Rigidbody2D _rb;

    [SerializeField]
    float _minDestructionSpeed;

    private void Start()
    {
        _carryableObject = GetComponentInChildren<SpriteRenderer>().gameObject;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(_destructibleObject && !collision.gameObject.GetComponent<PlayerController>())
        {
            _destructionEffect?.Play();
            _carryableObject?.SetActive(false);
        }
    }

    public void ReEnableVisuals()
    {
        _carryableObject?.SetActive(true);
    }
}
