using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    #region GlobalVariables
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
    #endregion

    #region hearts
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
    #endregion

    #region damage
    public virtual void TakeDamage(int damage)
    {
        IncrementHealth(damage);
        if (_health <= 0 && tag == "enemy")
        {
            Destroy(this.gameObject);
        }
    }
    #endregion 

    #region IncrementHealth
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
    #endregion


}
