using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    #region GlobalVariables
    [field: SerializeField]
    public int Health { get; private set; }

    [SerializeField]
    protected int _maxHealth;
    public int AmountOfHearts { get; private set; }

    [field: SerializeField]
    public int Damage { get; private set; }

    [field: SerializeField]
    public float Speed { get; private set; }

    [field: SerializeField]
    public float TimeToAttack { get; private set; }

    HeartDisplayHandler _heartDisplayHandler;

    [SerializeField]
    protected GameManager _gameManager;

    #endregion

    public virtual void Awake()
    {
        _heartDisplayHandler = GetComponentInChildren<HeartDisplayHandler>();
        _gameManager = FindObjectOfType<GameManager>();
        UpdateHeartAmount();
        Health = _maxHealth;
    }

    #region Health
    public virtual void UpdateHeartAmount()
    {
        AmountOfHearts = _maxHealth / 4;
    }
    public virtual void TakeDamage(int damage)
    {
        IncrementHealth(damage);
        if (Health <= 0 && tag == "enemy")
        {
            Destroy(this.gameObject);
        }
    } 
    public void HealEntity(int healthToHeal)
    {
        _heartDisplayHandler.IncreaseHeartQuarters(healthToHeal);
        Health += healthToHeal;
        Health = Mathf.Clamp(Health, 0, _maxHealth);
    }

    public void IncreaseHealth(int increaseAmount)
    {
        HealEntity(increaseAmount);
        UpdateHeartAmount();
        //_heartDisplayHandler.AddOneHeart();
    }

    public virtual void IncrementHealth(int incrementAmount)
    {

        if(Health <= 0)
        {
            return;
        }
        Health -= incrementAmount;
        Health = Mathf.Clamp (Health, 0 , _maxHealth);
        _heartDisplayHandler?.DecreaseHeartQuarters(incrementAmount);
    }
    

    public bool AtFullHealth()
    {
        return Health == _maxHealth;
    }
    #endregion
}
