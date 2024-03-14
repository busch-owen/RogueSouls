using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{

    [SerializeField]
    protected int _playerLevel;
    protected float _playerLevelProgression;
    protected float _amountUntilNextLevel;
    [SerializeField]
    protected float _levelProgressionMultiplier;
    [SerializeField]
    protected float _xpValue;

    public void IncrementPlayerLevel(int incrementAmount)
    {
        _playerLevelProgression += incrementAmount;

        if (_playerLevelProgression >= _amountUntilNextLevel)
        {
            _playerLevel++;
            _playerLevelProgression -= _amountUntilNextLevel;

            _amountUntilNextLevel *= _levelProgressionMultiplier;
        }
    }
}
