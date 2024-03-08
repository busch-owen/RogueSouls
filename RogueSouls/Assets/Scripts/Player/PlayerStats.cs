using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    [SerializeField]
    protected int _health;
    [SerializeField]
    protected int _maxHealth;

    [SerializeField]
    protected float _damage;

    [SerializeField]
    protected float _speed;

    [SerializeField]
    protected int _playerLevel;
    protected int _playerLevelProgression;

}
