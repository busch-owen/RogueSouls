using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RangedWeapon : MonoBehaviour
{

    //weapon basics

    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float bulletLifetime;
    [SerializeField]
    private int maxAmmo;
    [SerializeField]
    public float bulletCount;
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    public Camera cam;
    [SerializeField]
    ParticleSystem muzzleFlashEffect;
    [SerializeField]
    private GameObject impactEffect;    
    [SerializeField] 
	Transform firePoint;
	[SerializeField]
	Bullet bulletPrefab;
	[SerializeField]
	float bulletForce = 0.15f;
    [SerializeField]
    float minSpread;
    [SerializeField]
    float maxSpread;

    [SerializeField]
    AudioClip Reload_sounds;
    [SerializeField]
    AudioSource sfxHandler;
    [SerializeField]
    AudioClip gun_sounds;


    [SerializeField]
    string projectileName = "Projectile";

    public static RangedWeapon Instance;

    [SerializeField]
    public float damage;

    PlayerController playerController;

    bool shoot;

    //end of editable variables within the inspector

    private int currentAmmo;
    private bool isReloading = false;
    private float timeToNextFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;// always start with max ammo
        damage = 1.0f;
    }

    private void OnEnable()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAmmo == 0 && !isReloading)
        {
            Reload();
        }

        if (shoot && !playerController.CurrentlyRolling())
        {
            Shoot();
        }
    }

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

    public virtual void Shoot(Vector2 additionalVelocity = new Vector2())
    {
        if (Time.time >= timeToNextFire && !isReloading)// make sure you can't shoot faster than the gun allows to
        {
            timeToNextFire = Time.time + 1.0f / fireRate;// sets the time for the next bullet to be able to be fired

            currentAmmo--;
            //sfxHandler?.PlayOneShot(gun_sounds);


            Quaternion defaultSpreadAngle = firePoint.localRotation;
            float spread = Random.Range(minSpread, maxSpread);
            firePoint.transform.Rotate(new Vector3(0, 0, 1), -spread / 2f);
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = (float)spread / (float)(bulletCount);

                firePoint.transform.Rotate(new Vector3(0, 0, 1), angle);

                Bullet bullet = (Bullet)PoolManager.Instance.Spawn(projectileName);
                bullet.transform.position = firePoint.transform.position;
                bullet.transform.rotation = firePoint.transform.rotation;
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb?.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);//impulse force represents impact 
            }

            firePoint.localRotation = defaultSpreadAngle;
        }
    }

    void Reload()
    {

        isReloading = true;

       // sfxHandler.clip = Reload_sounds;
       // sfxHandler?.Play();


        //Reload_sfx?.Stop();
        //Reload_sfx?.PlayOneShot(Reload_sounds);
        Invoke("FinishReload", reloadTime); // we do an invoke so we can add a delay to the reload time, rather than a regular function call
    }

    void FinishReload()
    {
        currentAmmo = maxAmmo;
        //sfxHandler?.Stop();
        //sfxHandler.clip = null;
        isReloading = false;
    }

    public float AssignDamage( )
    {
        return damage;
    }
}
