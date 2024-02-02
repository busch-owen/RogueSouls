using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaponbase : MonoBehaviour
{

    //weapon basics
    [SerializeField]
    private float damage;
    [SerializeField]
    private float firerate;
    [SerializeField]
    private float range;
    [SerializeField]
    private int Maxammo;
    [SerializeField]
    public float bulletCount;
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    public Camera cam;
    [SerializeField]
    ParticleSystem Mflash;
    [SerializeField]
    private GameObject impacteffect;
        [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip shootsfx;
    [SerializeField]
    private AudioClip reloadsfx;

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


    //end of editable variables within the inspector

    private int currentAmmo;
    private bool isReloading = false;
    private float timeToNextFire = 0f;

    



    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = Maxammo;// always start with max ammo
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        return;

        if (currentAmmo == 0)
        {
            Reload();
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= timeToNextFire)// make sure you can't shoot faster than the gun allows to
        {
            timeToNextFire = Time.time + 1.0f / firerate;// sets the time for the next bullet to be able to be fired

            Shoot();
        }
    
    }


    public virtual void Shoot(Vector2 additionalVelocity = new Vector2())//virtual function can be overridden 
	{

        currentAmmo --;
		
        Quaternion defaultSpreadAngle = firePoint.localRotation;
        float spread = Random.Range(minSpread, maxSpread);
        Debug.LogFormat("Spread is {0}", spread);
        firePoint.transform.Rotate(new Vector3(0, 0, 1),-spread/2f);
        for(int i = 0; i < bulletCount; i++)
        {
            float angle = (float)spread/(float)(bulletCount);

            firePoint.transform.Rotate(new Vector3(0, 0, 1), angle);
            
            Bullet bullet = Instantiate<Bullet>(bulletPrefab, firePoint.position, firePoint.rotation);
		    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
		    rb?.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);//impulse force represents impact 
        }  

        firePoint.localRotation = defaultSpreadAngle;
	}


    void Reload()
    {
        isReloading = true;

        //audiosource go here audioSource.PlayOneShot(reloadsfx);

        Invoke("FinishReload", reloadTime); // we do an invoke so we can add a delay to the reload time, rather than a regular function call
    }

    void FinishReload()
    {
        currentAmmo = Maxammo;

        isReloading = false;

    }
}
