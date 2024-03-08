using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : Singleton<EntityStats>
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

    [SerializeField]
    protected int _playerLevel;
    protected float _playerLevelProgression;
    protected float _amountUntilNextLevel;
    [SerializeField]
    protected float _levelProgressionMultiplier;
    [SerializeField]
    protected float _xpValue;

    HeartDisplayHandler _heartDisplayHandler;

    private void Awake()
    {
        _heartDisplayHandler = GetComponentInChildren<HeartDisplayHandler>();
        UpdateHeartAmount();
        _health = _maxHealth;
    }

    public void IncrementPlayerLevel(int incrementAmount)
    {
        _playerLevelProgression += incrementAmount;

        if(_playerLevelProgression >= _amountUntilNextLevel)
        {
            _playerLevel++;
            _playerLevelProgression -= _amountUntilNextLevel;

            _amountUntilNextLevel *= _levelProgressionMultiplier;
        }
    }

    public void UpdateHeartAmount()
    {
        AmountOfHearts = _maxHealth / 4;
    }

    public void IncrementHealth(int incrementAmount)
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
