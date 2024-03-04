using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{

    //weapon basics
    [SerializeField]
    private float damage;
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
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip shootSFX;
    [SerializeField]
    private AudioClip reloadSFX;

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
    }

    private void OnEnable()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAmmo == 0)
        {
            Reload();
            return;
        }

        if (shoot && !playerController.CurrentlyRolling())
        {
            Shoot();
        }
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

            Quaternion defaultSpreadAngle = firePoint.localRotation;
            float spread = Random.Range(minSpread, maxSpread);
            firePoint.transform.Rotate(new Vector3(0, 0, 1), -spread / 2f);
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = (float)spread / (float)(bulletCount);

                firePoint.transform.Rotate(new Vector3(0, 0, 1), angle);

                Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb?.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);//impulse force represents impact 
            }

            firePoint.localRotation = defaultSpreadAngle;
        }
    }

    void Reload()
    {
        isReloading = true;

        //audiosource go here audioSource.PlayOneShot(reloadsfx);

        Invoke("FinishReload", reloadTime); // we do an invoke so we can add a delay to the reload time, rather than a regular function call
    }

    void FinishReload()
    {
        currentAmmo = maxAmmo;

        isReloading = false;
    }
}
