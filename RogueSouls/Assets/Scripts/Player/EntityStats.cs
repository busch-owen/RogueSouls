using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField]
    public int _health { get; private set; }
    [SerializeField]
    protected int _maxHealth;
    public int AmountOfHearts { get; private set; }

    [field: SerializeField]
    public int _damage { get; private set; }

    [field: SerializeField]
    public float _speed { get; private set; }

    [field: SerializeField]
    public float _timeToAttack { get; private set; }

    HeartDisplayHandler _heartDisplayHandler;

    public virtual void Awake()
    {
        _heartDisplayHandler = GetComponentInChildren<HeartDisplayHandler>();
        UpdateHeartAmount();
        _health = _maxHealth;
    }



    public virtual void UpdateHeartAmount()
    {
        AmountOfHearts = _maxHealth / 4;
    }

    public virtual void TakeDamage(int damage)
    {
        IncrementHealth(damage);
        if (_health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void IncreaseHealth(int increaseAmount)
    {
        _maxHealth += increaseAmount;
        _health = _maxHealth;
        UpdateHeartAmount();
        _heartDisplayHandler.AddOneHeart();
    }

    public virtual void IncrementHealth(int incrementAmount)
    {

        if(_health <= 0)
        {
            return;
        }
        _health -= incrementAmount;
        _health = Mathf.Clamp (_health, 0 , _maxHealth);
        _heartDisplayHandler?.DecrementHeartQuarters(incrementAmount);
    }


}
