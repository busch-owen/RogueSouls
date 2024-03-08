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

    [SerializeField]
    protected float _damage;

    [SerializeField]
    protected float _speed;

    [SerializeField]
    protected int _playerLevel;
    protected float _playerLevelProgression;
    protected float _amountUntilNextLevel;
    [SerializeField]
    protected float _levelProgressionMultiplier;

    private void Awake()
    {
        UpdateHeartAmount();
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
        _health -= incrementAmount;
    }

}
