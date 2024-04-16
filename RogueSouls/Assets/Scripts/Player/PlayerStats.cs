using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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

    VolumeProfile _postProcessVolume;
    Vignette vignette;

    ScreenShakeEffect _hurtScreenShake;

    public int MajorSoulsCollected { get; private set; }
    public int MinorSoulsCollected { get; private set; }


    #endregion
    protected override void Awake()
    {
        base.Awake();
        _postProcessVolume = FindObjectOfType<Volume>().profile;
        _cameraConfiner = FindObjectOfType<CinemachineConfiner2D>();
        _hurtScreenShake = GetComponent<ScreenShakeEffect>();
    }

    #region Damage
    public override void TakeDamage(int damage)
    {
        IncrementHealth(-damage);
        StartHurtEffect();
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

    void StartHurtEffect()
    {
        if (!_postProcessVolume.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        vignette.intensity.Override(0.5f);
        InvokeRepeating("DecreaseHurtEffect", 0, 0.02f);
        _hurtScreenShake.ShakeScreen();
    }

    void DecreaseHurtEffect() 
    {
        if (!_postProcessVolume.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        if(vignette.intensity.value > 0)
        {
            float newIntesity = vignette.intensity.value - 0.01f;
            vignette.intensity.Override(newIntesity);
        }
        else
        {
            CancelInvoke("DecreaseHurtEffect");
        }
    }

}