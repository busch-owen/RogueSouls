using UnityEngine;

public class BigRock : MonoBehaviour
{
    [SerializeField] private GameObject _explosionEffect;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.GetComponent<Explosion>()) return;
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
