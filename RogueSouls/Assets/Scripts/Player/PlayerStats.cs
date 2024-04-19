using Cinemachine;
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
    
    Transform _respawnPoint;

    protected float _xpValue;

    CinemachineConfiner2D _cameraConfiner;

    PolygonCollider2D _newCameraBounds;

    VolumeProfile _postProcessVolume;
    Vignette vignette;

    ScreenShakeEffect _hurtScreenShake;
    [SerializeField]
    private Youdied youDied;

    public bool PlayerIsDead { get; private set; }

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
            if(!PlayerIsDead)
            {
                PlayerIsDead = true;
                youDied.Died();
                _cameraConfiner.m_BoundingShape2D = _newCameraBounds;
            }            
        }
    }
    #endregion
    
    public void Respawn()
    {
        if(_respawnPoint != null)
        {
            transform.position = _respawnPoint.position;
            PlayerIsDead = false;
            IncrementHealth(999999);
        }
        else
        {
            transform.position = Vector3.zero;
            PlayerIsDead = false;
            IncrementHealth(999999);
        }
    }
    
    public void ChangeRespawnPoint(Transform newPoint)
    {
        _respawnPoint = newPoint;
    }

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