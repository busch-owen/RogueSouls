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

    BoxCollider2D _boxCollider;
    CircleCollider2D _circleCollider;

    Rigidbody2D _rb;

    private void Start()
    {
        _carryableObject = GetComponentInChildren<SpriteRenderer>().gameObject;
        _boxCollider = GetComponent<BoxCollider2D>();
        _circleCollider = GetComponent<CircleCollider2D>();

        _boxCollider.enabled = true;
        _circleCollider.enabled = true;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_destructibleObject && !collision.gameObject.GetComponent<PlayerController>())
        {
            SpawnHeartPickup();

            _destructionEffect?.Play();
            _carryableObject?.SetActive(false);
            _boxCollider.enabled = false;
            _circleCollider.enabled = false;
        }
    }

    public void ReEnableVisuals()
    {
        _carryableObject?.SetActive(true);
        _boxCollider.enabled = true;
        _circleCollider.enabled = true;
    }

    void SpawnHeartPickup()
    {
        int chanceToDropHeart = Random.Range(0, 4);
        if (chanceToDropHeart == 1)
        {
            PoolObject newPickup = PoolManager.Instance.Spawn("HealthPickup");
            newPickup.transform.position = this.transform.position;
        }
    }
}
