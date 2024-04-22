using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RangedWeapon : MonoBehaviour
{
    #region global Variables
    //weapon basics

    [SerializeField]
    protected float fireRate;
    [SerializeField]
    private float bulletLifetime;
    [field: SerializeField]
    public int MaxAmmo { get; private set; }
    [SerializeField]
    protected float bulletCount;
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    public Camera cam;
    [SerializeField]
    ParticleSystem muzzleFlashEffect;
    [SerializeField]
    private GameObject impactEffect;    
    [SerializeField] 
	public Transform firePoint;
	[SerializeField]
	public Bullet bulletPrefab;
	[SerializeField]
	protected float bulletForce = 0.15f;
    [SerializeField]
    float minSpread;
    [SerializeField]
    float maxSpread;

    protected UIHandler _uiHandler;

    ScreenShakeEffect _screenShakeEffect;

    [SerializeField]
    AudioClip Reload_sounds;
    [SerializeField]
    AudioSource sfxHandler;
    [SerializeField]
    AudioClip gun_sounds;

    [SerializeField]
    int damage;

    public PlayerController playerController { get; private set; }

    bool shoot;


    //end of editable variables within the inspector

    public int CurrentAmmo { get; private set; }
    private bool isReloading = false;
    [SerializeField]
    protected float timeToNextFire = 0f;
#endregion

#region first load

public virtual void Awake()
    {
        _uiHandler = FindObjectOfType<UIHandler>();
        sfxHandler = GetComponent<AudioSource>();
    }
    void Start()
    {   
        CurrentAmmo = MaxAmmo;// always start with max ammo
        _screenShakeEffect = GetComponent<ScreenShakeEffect>();
    }

    private void OnEnable()
    {
        playerController = GetComponentInParent<PlayerController>();
    }
#endregion
    #region Update
    void Update()
    {
        if (CurrentAmmo == 0 && !isReloading)
        {
            Reload();
        }

        if (shoot && !playerController.CurrentlyRolling())
        {
            Shoot();
        }
    }
    #endregion
    #region Misc
    private void FixedUpdate()
    {
        
    }

    public void EnableShootInput()//virtual function can be overridden 
	{
        shoot = true;
	}

    public void DisableShootInput()
    {
        shoot = false;
    }
#endregion
    #region Shoot
    public virtual void Shoot(Vector2 additionalVelocity = new Vector2())
    {
        if (gameObject.GetComponentInParent<PlayerController>())
        {
            if (Time.time >= timeToNextFire && !isReloading && !_uiHandler.IsPaused && !playerController.PreventingInput)// make sure you can't shoot faster than the gun allows to
            {
                timeToNextFire = Time.time + 1.0f / fireRate;// sets the time for the next bullet to be able to be fired

                CurrentAmmo--;
                
                if(sfxHandler)
                    sfxHandler.PlayOneShot(gun_sounds);

                muzzleFlashEffect?.Stop();
                muzzleFlashEffect?.Play();

                _screenShakeEffect?.ShakeScreen();

                Quaternion defaultSpreadAngle = firePoint.localRotation;
                float spread = Random.Range(minSpread, maxSpread);
                firePoint.transform.Rotate(new Vector3(0, 0, 1), -spread / 2f);
                for (int i = 0; i < bulletCount; i++)
                {
                    float angle = (float)spread / (float)(bulletCount);

                    firePoint.transform.Rotate(new Vector3(0, 0, 1), angle);

                    Bullet bullet = (Bullet)PoolManager.Instance.Spawn(bulletPrefab.name);
                    bullet.AssignWeapon(this);
                    bullet.GetComponent<TrailRenderer>().enabled = false;
                    bullet.transform.position = firePoint.transform.position;
                    bullet.transform.rotation = firePoint.transform.rotation;
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    rb.velocity = Vector2.zero;
                    rb?.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);//impulse force represents impact 
                    bullet.GetComponent<TrailRenderer>().enabled = true;
                }

                firePoint.localRotation = defaultSpreadAngle;
            }
        }
        else
        {
            if (Time.time >= timeToNextFire && !isReloading && !_uiHandler.IsPaused)// make sure you can't shoot faster than the gun allows to
            {
                timeToNextFire = Time.time + 1.0f / fireRate;// sets the time for the next bullet to be able to be fired

                CurrentAmmo--;
                //sfxHandler?.PlayOneShot(gun_sounds);

                muzzleFlashEffect?.Stop();
                muzzleFlashEffect?.Play();

                _screenShakeEffect?.ShakeScreen();

                Quaternion defaultSpreadAngle = firePoint.localRotation;
                float spread = Random.Range(minSpread, maxSpread);
                firePoint.transform.Rotate(new Vector3(0, 0, 1), -spread / 2f);
                for (int i = 0; i < bulletCount; i++)
                {
                    float angle = (float)spread / (float)(bulletCount);

                    firePoint.transform.Rotate(new Vector3(0, 0, 1), angle);

                    Bullet bullet = (Bullet)PoolManager.Instance.Spawn(bulletPrefab.name);
                    bullet.AssignWeapon(this);

                    if(bullet.GetComponent<TrailRenderer>())
                        bullet.GetComponent<TrailRenderer>().enabled = false;

                    bullet.transform.position = firePoint.transform.position;
                    bullet.transform.rotation = firePoint.transform.rotation;
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    if(rb != null)
                    {
                        rb.velocity = Vector2.zero;
                        rb?.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);//impulse force represents impact 
                    }
                    if (bullet.GetComponent<TrailRenderer>())
                        bullet.GetComponent<TrailRenderer>().enabled = true;
                }

                firePoint.localRotation = defaultSpreadAngle;
            }
        }
        
    }
#endregion
    #region Reload
    public void Reload()
    {
        if(CurrentAmmo != MaxAmmo)
        {
            isReloading = true;

            if(GetComponentInParent<PlayerController>())
            {
                _uiHandler.EnableReloadingText(reloadTime);
            }
            // sfxHandler.clip = Reload_sounds; // this is commented out until we add back sfx, it was causing errors with not every weapon having one 
            // sfxHandler?.Play();


            //Reload_sfx?.Stop();
            //Reload_sfx?.PlayOneShot(Reload_sounds);
            Invoke("FinishReload", reloadTime); // we do an invoke so we can add a delay to the reload time, rather than a regular function call
        }
    }


    public virtual void FinishReload()
    {
        CurrentAmmo = MaxAmmo;
        //sfxHandler?.Stop();
        //sfxHandler.clip = null;
        isReloading = false;
    }
    #endregion
    #region Damage
    public int AssignDamage( )
    {
        return damage;
    }
}
#endregion
