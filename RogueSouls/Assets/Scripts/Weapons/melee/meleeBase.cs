using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class meleeBase : MonoBehaviour
{

    [SerializeField]
    private float damage;
    [SerializeField]
    private float range;
    [SerializeField]
    private float knockback;
    [SerializeField]
    private float stamina;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private float swingSpeed;
    [SerializeField]
    private Animator animator;
    private Collider2D hitbox;

    [SerializeField]
    GameObject melee;
    [SerializeField]
    float meleeOffset;
    //

    private bool isSwinging = false;
    [SerializeField]
    private float timeTonextSwing;
    private float currentStamina;

    void Start()
    {
        currentStamina = stamina;
    }

    
    void Update()
    {
        if (isSwinging)
        {
            return;
            
           
        }

         if (Input.GetButton("Fire2") && Time.time >= timeTonextSwing)
            {
                timeTonextSwing = Time.time + cooldown;
                
                Swing();
            }
    }

    void Swing()
    {
        if (currentStamina == 0)
        {
            return;
        }

        isSwinging = true;

        animator.SetTrigger("Swing");

        currentStamina --;

        Invoke("StopSwing", swingSpeed);    
    }

    void StopSwing()
    {
        isSwinging = false;

    }

    void DisableHitbox()
    {
        hitbox.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
            Vector2 direction = other.transform.position - transform.position;
            direction.Normalize();
            other.GetComponent<Rigidbody2D>().AddForce(direction * knockback, ForceMode2D.Impulse);
    }

    void MeleeOffsetWeaponPos(float rotationAngle)
    {
        melee.transform.localPosition = new Vector2(this.transform.localPosition.x + meleeOffset, this.transform.localPosition.y);

        if (rotationAngle is < 90 and > -180 && rotationAngle is not < -90 and > -180)
        {
            melee.transform.localScale = new Vector3(melee.transform.localScale.x, 1, melee.transform.localScale.z);
        }
        else
        {
            melee.transform.localScale = new Vector3(melee.transform.localScale.x, -1, melee.transform.localScale.z);
        }
    }
}
