using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    #region Global Variables
    [SerializeField]
    protected int _playerLevel;
    protected float _playerLevelProgression;
    protected float _amountUntilNextLevel;
    [SerializeField]
    protected float _levelProgressionMultiplier;
    [SerializeField]

    protected float _xpValue;
    #endregion
    #region Level
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
    #endregion
    #region Damage
    public override void TakeDamage(int damage)
    {
        IncrementHealth(-damage);
        if (Health <= 0)
        {
            _gameManager.Restart();
        }
  
    }
    #endregion

}