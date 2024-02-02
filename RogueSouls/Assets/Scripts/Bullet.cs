using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
	GameObject hitEffect;

	// Start is called before the first frame update
	private void OnCollisionEnter2D(Collision2D other) 
	{
        
        
            
        if(hitEffect)
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
		    Destroy(effect, 2f);
        }

		Destroy(gameObject);
	}
}
