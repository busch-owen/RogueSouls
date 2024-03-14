using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSoul : MonoBehaviour
{
    HUD _targetHUD;

    [SerializeField]
    string _collectionMessage;

    [SerializeField]
    float _messageDuration;

    private void OnEnable()
    {
        _targetHUD = FindObjectOfType<HUD>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<PlayerStats>())
        {
            PlayerStats targetPlayer = other.GetComponent<PlayerStats>();
            targetPlayer.IncreaseHealth(4);
            Destroy(this.gameObject);

            _targetHUD.ShowSpecificMessage(_collectionMessage, _messageDuration);
        }
    }
}
