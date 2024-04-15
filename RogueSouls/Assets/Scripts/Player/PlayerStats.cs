using Cinemachine;
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

    CinemachineConfiner2D _cameraConfiner;

    PolygonCollider2D _newCameraBounds;

    public int MajorSoulsCollected { get; private set; }
    public int MinorSoulsCollected { get; private set; }


    #endregion
    protected override void Awake()
    {
        base.Awake();
        _cameraConfiner = FindObjectOfType<CinemachineConfiner2D>();
    }

    #region Damage
    public override void TakeDamage(int damage)
    {
        IncrementHealth(-damage);
        if (Health <= 0)
        {
            Respawn();
            _cameraConfiner.m_BoundingShape2D = _newCameraBounds;
        }
    }
    #endregion

    public void SetRespawnCameraBounds(PolygonCollider2D newBounds)
    {
        _newCameraBounds = newBounds;
    }

    public void IncreaseMajorSoulCount()
    {
        MajorSoulsCollected++;
    }

    public void IncreaseMinorSoulCount()
    {
        MinorSoulsCollected++;
    }
}